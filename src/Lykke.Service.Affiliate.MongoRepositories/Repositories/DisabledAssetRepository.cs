using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class DisabledAssetEntity : MongoEntity, IDisabledAsset
    {
        public string AssetId => BsonId;

        public static DisabledAssetEntity Create(string assetId)
        {
            return new DisabledAssetEntity
            {
                BsonId = assetId
            };
        }
    }

    public class DisabledAssetRepository : IDisabledAssetRepository
    {
        private readonly IMongoStorage<DisabledAssetEntity> _storage;

        public DisabledAssetRepository(IMongoStorage<DisabledAssetEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IDisabledAsset>> GetAll()
        {
            return await _storage.GetDataAsync();
        }

        public async Task<IDisabledAsset> CreateAsync(string assetId)
        {
            var item = DisabledAssetEntity.Create(assetId);
            await _storage.InsertOrReplaceAsync(item);
            return item;
        }

        public Task DeleteAsync(string assetId)
        {
            return _storage.DeleteAsync(assetId);
        }
    }
}
