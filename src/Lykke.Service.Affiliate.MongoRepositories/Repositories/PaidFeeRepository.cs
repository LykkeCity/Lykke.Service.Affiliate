using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class PaidFeeEntity : MongoEntity, IPaidFee
    {
        [BsonIgnore]
        public string Id => BsonId;

        [BsonIgnore]
        public DateTime Date => BsonCreateDt;

        public string AssetId { get; set; }
        public string FromClient { get; set; }
        public string ToClient { get; set; }
        public decimal Volume { get; set; }
        public string Order { get; set; }
        public string TradeClient { get; set; }
        public string TradeOppositeClient { get; set; }
        public decimal TradeVolume { get; set; }

        public static PaidFeeEntity Create(string id, string assetId, string fromClientId, string toClientId, decimal volume, string orderId, string tradeClient, string tradeOppositeClient, decimal tradeVolume)
        {
            return new PaidFeeEntity
            {
                BsonId = id,
                AssetId = assetId,
                FromClient = fromClientId,
                ToClient = toClientId,
                Volume = volume,
                Order = orderId,
                TradeClient = tradeClient,
                TradeOppositeClient = tradeOppositeClient,
                TradeVolume = tradeVolume
            };
        }
    }

    public class PaidFeeRepository : IPaidFeeRepository
    {
        private readonly IMongoStorage<PaidFeeEntity> _table;

        public PaidFeeRepository(IMongoStorage<PaidFeeEntity> table)
        {
            _table = table;
        }

        public Task Create(string id, string assetId, string fromClientId, string toClientId, decimal volume, string orderId, string tradeClient, string tradeOppositeClient, decimal tradeVolume)
        {
            return _table.InsertOrReplaceAsync(PaidFeeEntity.Create(id, assetId, fromClientId, toClientId, volume,
                orderId, tradeClient, tradeOppositeClient, tradeVolume));
        }
    }
}
