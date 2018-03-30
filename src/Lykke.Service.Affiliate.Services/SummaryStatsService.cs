using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;

namespace Lykke.Service.Affiliate.Services
{
    public class SummaryStatsService : ISummaryStatsService
    {
        private readonly IBonusAccrualRepository _bonusAccrualRepository;
        private readonly IClientAccrualRepository _clientAccrualRepository;

        public SummaryStatsService(
            IBonusAccrualRepository bonusAccrualRepository,
            IClientAccrualRepository clientAccrualRepository
            )
        {
            _bonusAccrualRepository = bonusAccrualRepository;
            _clientAccrualRepository = clientAccrualRepository;
        }
        public async Task<IEnumerable<StatisticsItem>> GetSummaryStatsAsync()
        {
            IEnumerable<IClientAccrual> data = await _clientAccrualRepository.GetClientAccruals();
            
            IEnumerable<StatisticsItem> result =  data.GroupBy(x => x.AssetId).Select(x => new StatisticsItem
            {
                AssetId = x.Key,
                BonusVolume = x.Sum(o => o.Bonus),
                TradeVolume = x.Sum(o => o.TradeVolume)
            });

            var todayBonuses = await GetSummaryStatsAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));

            return result.Concat(todayBonuses).OrderBy(item => item.AssetId);
        }

        public async Task<IEnumerable<StatisticsItem>> GetSummaryStatsAsync(DateTime startDate, DateTime endDate)
        {
            IEnumerable<IBonusAccrual> data = await _bonusAccrualRepository.GetData(startDate, endDate);
            
            return data.GroupBy(x => x.AssetId).Select(x => new StatisticsItem
            {
                AssetId = x.Key,
                BonusVolume = x.Sum(o => o.Bonus),
                TradeVolume = x.Sum(o => o.TradeVolume)
            }).OrderBy(item => item.AssetId);
        }
    }
}
