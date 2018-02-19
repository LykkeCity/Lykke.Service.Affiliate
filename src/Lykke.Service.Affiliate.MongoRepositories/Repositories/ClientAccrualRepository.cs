using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class ClientAccrualEntity : MongoEntity, IClientAccrual
    {
        [BsonIgnore]
        public string Id => BsonId;

        public string MeId { get; set; }
        public string AccrualPeriodId { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public decimal Bonus { get; set; }
        public bool Completed { get; set; }

        public static string GetId(string accrualPeriodId, string assetId)
        {
            return $"{accrualPeriodId}_{assetId}";
        }

        public static ClientAccrualEntity Create(string accrualPeriodId, string clientId, string assetId, decimal bonus)
        {
            return new ClientAccrualEntity
            {
                BsonId = GetId(accrualPeriodId, assetId),
                AssetId = assetId,
                ClientId = clientId,
                Bonus = bonus,
                MeId = Guid.NewGuid().ToString()
            };
        }
    }

    public class ClientAccrualRepository : IClientAccrualRepository
    {
        private readonly IMongoStorage<ClientAccrualEntity> _storage;

        public ClientAccrualRepository(IMongoStorage<ClientAccrualEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IClientAccrual> Create(string accrualPeriodId, string clientId, string assetId, decimal bonus)
        {
            var entity = ClientAccrualEntity.Create(accrualPeriodId, clientId, assetId, bonus);
            await _storage.InsertAsync(entity);
            return entity;
        }

        public async Task<IClientAccrual> GetClientAccrual(string accrualPeriodId, string asset)
        {
            return await _storage.GetDataAsync(ClientAccrualEntity.GetId(accrualPeriodId, asset));
        }

        public Task SetCompleted(string id)
        {
            return _storage.MergeAsync(id, x =>
            {
                x.Completed = true;
                return x;
            });
        }
    }
}
