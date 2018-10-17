using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.Affiliate.Services
{
    public class AffiliateService : IAffiliateService
    {
        private readonly ILinkRedirectRepository _linkRedirectRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly ILinkRepository _linkRepository;
        private readonly ILog _log;
        private readonly IMemoryCache _memoryCache;

        public AffiliateService(ILinkRedirectRepository linkRedirectRepository, IReferralRepository referralRepository, ILogFactory logFactory, ILinkRepository linkRepository, IMemoryCache memoryCache)
        {
            _linkRedirectRepository = linkRedirectRepository;
            _referralRepository = referralRepository;
            _log = logFactory.CreateLog(this);
            _linkRepository = linkRepository;
            _memoryCache = memoryCache;
        }

        public async Task Register(string ip, string clientId)
        {
            var record = await _linkRedirectRepository.GetRedirect(ip);

            if (record == null || DateTime.UtcNow > record.ExpirationDt)
                return;

            var item = await _referralRepository.SaveReferral(clientId, record.AffiliateId);

            _memoryCache.Set(Constants.GetCacheReferralKey(clientId), item);

            _log.Info(nameof(Register), $"New referral {clientId} for {record.AffiliateId} registered. (Link expiration {record.ExpirationDt})");
        }

        public async Task<IEnumerable<string>> GetAllAffiliates()
        {
            var data = await _referralRepository.GetAllReferrals();

            return data.Select(x => x.AffiliateId).Distinct();
        }
    }
}
