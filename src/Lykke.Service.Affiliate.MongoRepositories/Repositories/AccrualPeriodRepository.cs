using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class AccrualPeriodEntity : MongoEntity, IAccrualPeriod
    {
        [BsonIgnore]
        public string Id => BsonId;
        public string ClientId { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public bool Completed { get; set; }

        public static AccrualPeriodEntity Create(string clientId, DateTime startDt, DateTime endDt)
        {
            return new AccrualPeriodEntity
            {
                BsonId = Guid.NewGuid().ToString(),
                ClientId = clientId,
                StartDt = startDt,
                EndDt = endDt
            };
        }
    }

    public class AccrualPeriodRepository : IAccrualPeriodRepository
    {
        private readonly IMongoStorage<AccrualPeriodEntity> _storage;

        public AccrualPeriodRepository(IMongoStorage<AccrualPeriodEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IAccrualPeriod> GetLastPeriod(string clientId)
        {
            return await _storage.GetTopRecordAsync(x => x.ClientId == clientId, x => x.EndDt, MongoDB.Driver.SortDirection.Descending);
        }

        public async Task<IAccrualPeriod> CreatePeriod(string clientId, DateTime startDt, DateTime endDt)
        {
            var item = AccrualPeriodEntity.Create(clientId, startDt, endDt);
            await _storage.InsertAsync(item);
            return item;
        }

        public Task SetCompleted(string accrualPeriodId)
        {
            return _storage.MergeAsync(accrualPeriodId, x =>
            {
                x.Completed = true;
                return x;
            });
        }
    }
}
