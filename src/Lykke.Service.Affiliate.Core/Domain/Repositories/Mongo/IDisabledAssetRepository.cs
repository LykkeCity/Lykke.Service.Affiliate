using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IDisabledAsset
    {
        string AssetId { get; }
    }

    public interface IDisabledAssetRepository
    {
        Task<IEnumerable<IDisabledAsset>> GetAll();
        Task<IDisabledAsset> CreateAsync(string assetId);
        Task DeleteAsync(string assetId);
    }
}
