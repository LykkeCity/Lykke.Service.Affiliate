
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Contracts;

namespace Lykke.Service.Affiliate.Client
{
    public interface IAffiliateClient
    {
        /// <summary>
        /// Create new referral link
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        Task<LinkModel> RegisterLink(string clientId, string redirectUrl);

        /// <summary>
        /// Get all links by client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<LinkModel>> GetLinks(string clientId);
        
        /// <summary>
        /// Get all client referrals
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<ReferralModel>> GetReferrals(string clientId);

        /// <summary>
        /// Get client bonus statistics
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<StatisticItemModel>> GetStats(string clientId);

    }
}
