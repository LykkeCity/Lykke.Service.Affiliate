using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;

namespace Lykke.Service.Affiliate.Services
{
    public class AccrualService : IAccrualService
    {
        private readonly IClientAccrualRepository _clientAccrualRepository;

        public AccrualService(IClientAccrualRepository clientAccrualRepository)
        {
            _clientAccrualRepository = clientAccrualRepository;
        }

        public async Task<IEnumerable<StatisticsItem>> GetStats(string clientId)
        {
            var data = await _clientAccrualRepository.GetClientAccruals(clientId);

            return data.GroupBy(x => x.AssetId).Select(x => new StatisticsItem
            {
                AssetId = x.Key,
                BonusVolume = x.Sum(o => o.Bonus),
                TradeVolume = x.Sum(o => o.TradeVolume)
            });
        }
    }
}
