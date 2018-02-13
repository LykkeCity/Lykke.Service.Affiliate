using System;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface ILinkRedirect
    {
        string AffiliateId { get; }
        string Ip { get; }
        string LinkId { get; }
    }

    public interface ILinkRedirectRepository
    {
        Task SaveRedirect(string ip, string affiliateId, string linkId, TimeSpan ipCache);
        Task<ILinkRedirect> GetRedirect(string ip);
    }
}
