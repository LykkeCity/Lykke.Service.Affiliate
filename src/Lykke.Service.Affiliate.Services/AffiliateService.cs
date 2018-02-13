using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;

namespace Lykke.Service.Affiliate.Services
{
    public class AffiliateService : IAffiliateService
    {
        private readonly ILinkRedirectRepository _linkRedirectRepository;
        private readonly IReferralRepository _referralRepository;
        private readonly ILog _logger;

        public AffiliateService(ILinkRedirectRepository linkRedirectRepository, IReferralRepository referralRepository, ILog logger)
        {
            _linkRedirectRepository = linkRedirectRepository;
            _referralRepository = referralRepository;
            _logger = logger;
        }

        public async Task Register(string ip, string clientId)
        {
            var record = await _linkRedirectRepository.GetRedirect(ip);

            if (record == null)
                return;

            await _referralRepository.SaveReferral(clientId, record.AffiliateId);

            await _logger.WriteInfoAsync(nameof(AffiliateService), nameof(Register), $"New referral {clientId} for {record.AffiliateId} registered");
        }
    }
}
