using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureStorage.Queue;
using Common;
using Common.Log;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;

namespace Lykke.Service.Affiliate.AzureRepositories
{
    public class PaidFeeQueueWriter : IPaidFeeQueueWriter
    {
        private readonly IQueueExt _queue;

        public PaidFeeQueueWriter(IQueueExt queue)
        {
            _queue = queue;
        }

        public async Task AddPaidFee(Guid id, string asset, string fromClient, string toClient, decimal volume, DateTime date, string order, string tradeClient, string tradeOppositeClient, decimal tradeVolume)
        {
            var msg = new PaidFeeQueueItem
            {
                AssetId = asset,
                Date = date,
                FromClient = fromClient,
                Id = id,
                Order = order,
                ToClient = toClient,
                Volume = volume,
                TradeClient = tradeClient,
                TradeOppositeClient = tradeOppositeClient,
                TradeVolume = tradeVolume
            };

            await _queue.PutRawMessageAsync(msg.ToJson());
        }
    }
}
