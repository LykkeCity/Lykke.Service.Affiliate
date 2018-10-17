using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class DisabledAssetController : Controller
    {
        private readonly IDisabledAssetRepository _disabledAssetRepository;
        private readonly IMemoryCache _memoryCache;

        public DisabledAssetController(IDisabledAssetRepository disabledAssetRepository, IMemoryCache memoryCache)
        {
            _disabledAssetRepository = disabledAssetRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet("list")]
        [SwaggerOperation("GetDIsabledAssets")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<string>> List()
        {
            var result = await _disabledAssetRepository.GetAll();

            return result.Select(x => x.AssetId);
        }

        [HttpPost]
        [SwaggerOperation("AddDisabledAsset")]
        public async Task<IActionResult> Create([FromBody]AddDisabledAssetModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.AssetId))
                return BadRequest();

            var item = await _disabledAssetRepository.CreateAsync(model.AssetId);

            _memoryCache.Set(Constants.GetCacheDisabledAssetKey(item.AssetId), item);

            return Ok();
        }

        [HttpDelete]
        [SwaggerOperation("RemoveDisabledAsset")]
        public async Task<IActionResult> Remove([FromQuery]string assetId)
        {
            await _disabledAssetRepository.DeleteAsync(assetId);

            _memoryCache.Remove(Constants.GetCacheDisabledAssetKey(assetId));

            return Ok();
        }
    }
}
