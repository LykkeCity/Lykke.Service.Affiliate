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
            
            List<StatisticsItem> result =  data.GroupBy(x => x.AssetId).Select(x => new StatisticsItem
            {
                AssetId = x.Key,
                BonusVolume = x.Sum(o => o.Bonus),
                TradeVolume = x.Sum(o => o.TradeVolume)
            }).ToList();

            var todayBonuses = await GetSummaryStatsAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));

            foreach (var bonus in todayBonuses)
            {
                var existing = result.FirstOrDefault(item => item.AssetId == bonus.AssetId);

                if (existing != null)
                {
                    existing.BonusVolume += bonus.BonusVolume;
                    existing.TradeVolume += bonus.TradeVolume;
                }
                else
                {
                    result.Add(bonus);
                }
            }
            
            return result.OrderBy(item => item.AssetId);
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
