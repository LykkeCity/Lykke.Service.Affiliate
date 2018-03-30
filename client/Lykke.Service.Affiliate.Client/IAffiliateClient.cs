
using System;
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
        
        /// <summary>
        /// Get total affiliates count
        /// </summary>
        /// <returns></returns>
        Task<int> GetAffiliatesCountAsync();
        
        /// <summary>
        /// Get total referrals count
        /// </summary>
        /// <returns></returns>
        Task<int> GetReferralsCountAsync();
        
        /// <summary>
        /// Get total bonus statistics
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<StatisticItemModel>> GetSummaryStatsAsync();
        
        /// <summary>
        /// Get total bonus statistics for a period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<IEnumerable<StatisticItemModel>> GetSummaryStatsForPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
