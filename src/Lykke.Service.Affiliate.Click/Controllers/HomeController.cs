using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Extensions;
using Lykke.Common.Log;
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
        private readonly ILog _log;

        public HomeController(
            ILinkRepository linkRepository, 
            ILinkRedirectRepository linkRedirectRepository, 
            RedirectIpCacheSetting setting, 
            ILogFactory logFactory)
        {
            _linkRepository = linkRepository;
            _linkRedirectRepository = linkRedirectRepository;
            _setting = setting;
            _log = logFactory.CreateLog(this);
        }

        [HttpGet("{key}")]
        [SwaggerOperation("Redirect")]
        public async Task<IActionResult> RegisterLink(string key)
        {
            var redirectLink = await _linkRepository.GetAsync(key);

            if (redirectLink == null)
                return NotFound();

            LogHeaders();

            await _linkRedirectRepository.SaveRedirect(HttpContext.GetIp(), redirectLink.AffiliateId, key, _setting.IpCacheTime);

            return Redirect(redirectLink.RedirectUrl);
        }

        private void LogHeaders()
        {
            try
            {
                var values = string.Join(",", HttpContext.Request.Headers.Values.Select(x => x.ToString()));
                var keys = string.Join(",", HttpContext.Request.Headers.Keys);
                _log.Info(nameof(RegisterLink), values, keys);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (System.Exception)
            {
            }
        }
    }
}
