using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface IReferralService
    {
        Task<IEnumerable<IReferral>> GetAllReferralsAsync();
        Task<int> GetReferralsCountAsync();
        Task<IEnumerable<IReferral>> GetReferralsAsync(string partnerId);
    }
}
