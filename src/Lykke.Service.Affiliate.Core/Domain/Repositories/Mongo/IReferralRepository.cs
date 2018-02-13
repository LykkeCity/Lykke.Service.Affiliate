using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IReferralEntity
    {
        string ReferralId { get; }
        string AffiliateId { get; }
    }

    public interface IReferralRepository
    {
        Task SaveReferral(string clientId, string affiliateId);
    }
}
