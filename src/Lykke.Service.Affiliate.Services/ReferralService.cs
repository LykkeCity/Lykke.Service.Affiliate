using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;

namespace Lykke.Service.Affiliate.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _referralRepository;

        public ReferralService(IReferralRepository referralRepository)
        {
            _referralRepository = referralRepository;
        }

        public async Task<IEnumerable<IReferral>> GetAllReferrals()
        {
            return await _referralRepository.GetAllReferrals();
        }
    }
}
