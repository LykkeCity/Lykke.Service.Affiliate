using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface ISummaryStatsService
    {
        Task<IEnumerable<StatisticsItem>> GetSummaryStatsAsync();
        Task<IEnumerable<StatisticsItem>> GetSummaryStatsAsync(DateTime startDate, DateTime endDate);
    }
}
