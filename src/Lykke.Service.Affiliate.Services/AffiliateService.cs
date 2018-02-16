using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
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
        private readonly ILog _logger;
        private readonly IMemoryCache _memoryCache;

        public AffiliateService(ILinkRedirectRepository linkRedirectRepository, IReferralRepository referralRepository, ILog logger, ILinkRepository linkRepository, IMemoryCache memoryCache)
        {
            _linkRedirectRepository = linkRedirectRepository;
            _referralRepository = referralRepository;
            _logger = logger;
            _linkRepository = linkRepository;
            _memoryCache = memoryCache;
        }

        public async Task Register(string ip, string clientId)
        {
            var record = await _linkRedirectRepository.GetRedirect(ip);

            if (record == null)
                return;

            var item = await _referralRepository.SaveReferral(clientId, record.AffiliateId);

            _memoryCache.Set(Constants.GetCacheReferralKey(clientId), item);

            await _logger.WriteInfoAsync(nameof(AffiliateService), nameof(Register), $"New referral {clientId} for {record.AffiliateId} registered");
        }

        public async Task<IEnumerable<string>> GetAllAffiliates()
        {
            var data = await _linkRepository.GetAllLinks();

            return data.Select(x => x.AffiliateId).Distinct();
        }
    }
}
