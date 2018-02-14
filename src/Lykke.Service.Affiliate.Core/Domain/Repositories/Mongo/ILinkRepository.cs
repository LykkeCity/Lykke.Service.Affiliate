using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface ILink
    {
        string AffiliateId { get; }
        string Key { get; }
        string RedirectUrl { get; }
    }

    public interface ILinkRepository
    {
        Task<ILink> CreateAsync(string affiliateId, string redirectUrl);
        Task<ILink> GetAsync(string key);
        Task<IEnumerable<ILink>> GetLinks(string clientId);
    }
}
