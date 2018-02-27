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
    }

    public class DisabledAssetRepository : IDisabledAssetRepository
    {
        private readonly IMongoStorage<DisabledAssetEntity> _storage;

        public DisabledAssetRepository(IMongoStorage<DisabledAssetEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IDisabledAsset>> GetDisabledAssets()
        {
            return await _storage.GetDataAsync();
        }
    }
}
