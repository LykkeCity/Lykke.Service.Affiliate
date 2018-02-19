﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class BonusAccrualEntity : MongoEntity, IBonusAccrual
    {
        [BsonIgnore]
        public DateTime CreatedDt => BsonCreateDt;

        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public decimal Amount { get; set; }
        public string PaidFeeId { get; set; }

        public static BonusAccrualEntity Create(string paidFeeId, string clientId, string assetId, decimal amount)
        {
            return new BonusAccrualEntity
            {
                BsonId = $"{paidFeeId}_{clientId}",
                PaidFeeId = paidFeeId,
                ClientId = clientId,
                Amount = amount,
                AssetId = assetId
            };
        }
    }

    public class BonusAccrualRepository : IBonusAccrualRepository
    {
        private readonly IMongoStorage<BonusAccrualEntity> _table;

        public BonusAccrualRepository(IMongoStorage<BonusAccrualEntity> table)
        {
            _table = table;
        }

        public Task Create(string paidFeeId, string clientId, string assetId, decimal amount)
        {
            return _table.InsertOrReplaceAsync(BonusAccrualEntity.Create(paidFeeId, clientId, assetId, amount));
        }

        public async Task<IEnumerable<IBonusAccrual>> GetData(string affiliateId, DateTime startDt, DateTime endDt)
        {
            return await _table.GetDataAsync(x => x.ClientId == affiliateId && x.CreatedDt >= startDt && x.CreatedDt < endDt);
        }
    }
}
