using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<IReferral>> GetAllReferralsAsync()
        {
            return await _referralRepository.GetAllReferrals();
        }

        public async Task<int> GetReferralsCountAsync()
        {
            return (await _referralRepository.GetAllReferrals()).Count();
        }

        public async Task<IEnumerable<IReferral>> GetReferralsAsync(string partnerId)
        {
            return await _referralRepository.GetReferrals(partnerId);
        }
    }
}
