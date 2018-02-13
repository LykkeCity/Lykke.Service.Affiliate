using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IReferalBonus
    {
        string AffiliateId { get; }
        string ClientId { get; }
        string AssetId { get; }
        decimal Amount { get; }
        string OrderId { get; }
    }

    public interface IReferralBonusRepository
    {
        Task Create(string affiliateId, string clientId, string assetId, decimal amount, string orderId);
    }
}
