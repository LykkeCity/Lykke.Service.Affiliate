using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IPaidFee
    {
        string Id { get; }
        string AssetId { get; set; }
        string FromClient { get; set; }
        string ToClient { get; set; }
        decimal Volume { get; set; }
        DateTime Date { get; }
        string Order { get; set; }
        string TradeClient { get; set; }
        string TradeOppositeClient { get; set; }
    }

    public interface IPaidFeeRepository
    {
        Task Create(string id, string assetId, string fromClientId, string toClientId, decimal volume,
            string orderId, string tradeClient, string tradeOppositeClient);
    }
}
