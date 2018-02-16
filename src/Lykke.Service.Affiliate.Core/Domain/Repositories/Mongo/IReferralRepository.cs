using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IReferral
    {
        string ReferralId { get; }
        string AffiliateId { get; }
        DateTime CreatedDt { get; }
    }

    public interface IReferralRepository
    {
        Task<IReferral> SaveReferral(string clientId, string affiliateId);
        Task<IEnumerable<IReferral>> GetReferrals(string partnerId);
        Task<IEnumerable<IReferral>> GetAllReferrals();
    }
}
