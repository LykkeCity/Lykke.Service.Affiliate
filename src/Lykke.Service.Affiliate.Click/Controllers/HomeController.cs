using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Extensions;
using Lykke.Service.Affiliate.Click.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Click.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ILinkRedirectRepository _linkRedirectRepository;
        private readonly RedirectIpCacheSetting _setting;
        private readonly ILog _logger;

        public HomeController(ILinkRepository linkRepository, ILinkRedirectRepository linkRedirectRepository, RedirectIpCacheSetting setting, ILog logger)
        {
            _linkRepository = linkRepository;
            _linkRedirectRepository = linkRedirectRepository;
            _setting = setting;
            _logger = logger;
        }

        [HttpGet("{key}")]
        [SwaggerOperation("Redirect")]
        public async Task<IActionResult> RegisterLink(string key)
        {
            var redirectLink = await _linkRepository.GetAsync(key);

            if (redirectLink == null)
                return NotFound();

            await LogHeaders();

            await _linkRedirectRepository.SaveRedirect(HttpContext.GetIp(), redirectLink.AffiliateId, key, _setting.IpCacheTime);

            return Redirect(redirectLink.RedirectUrl);
        }

        private async Task LogHeaders()
        {
            try
            {
                var values = string.Join(",", HttpContext.Request.Headers.Values.Select(x => x.ToString()));
                var keys = string.Join(",", HttpContext.Request.Headers.Keys);
                await _logger.WriteInfoAsync(nameof(RegisterLink), values, keys);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (System.Exception)
            {
            }
        }
    }
}
