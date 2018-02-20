using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Contracts
{
    public class StatisticItemModel
    {
        /// <summary>
        /// Lykke asset id
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Trade volume
        /// </summary>
        public decimal TradeVolume { get; set; }

        /// <summary>
        /// Fee bonus volume
        /// </summary>
        public decimal BonusVolume { get; set; }
    }
}
