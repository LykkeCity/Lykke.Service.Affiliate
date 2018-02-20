using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services
{
    public class StatisticsItem
    {
        public string AssetId { get; set; }
        public decimal BonusVolume { get; set; }
        public decimal TradeVolume { get; set; }
    }

    public interface IAccrualService
    {
        Task<IEnumerable<StatisticsItem>> GetStats(string clientId);
    }
}
