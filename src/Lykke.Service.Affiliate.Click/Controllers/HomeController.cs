using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Common.Extensions;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Models;
using Lykke.Service.Affiliate.Settings.ServiceSettings;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ILinkRedirectRepository _linkRedirectRepository;
        private readonly RedirectIpCacheSetting _setting;

        public HomeController(ILinkRepository linkRepository, ILinkRedirectRepository linkRedirectRepository, RedirectIpCacheSetting setting)
        {
            _linkRepository = linkRepository;
            _linkRedirectRepository = linkRedirectRepository;
            _setting = setting;
        }

        [HttpGet("{key}")]
        [SwaggerOperation("Redirect")]
        public async Task<IActionResult> RegisterLink(string key)
        {
            var redirectLink = await _linkRepository.GetAsync(key);

            if (redirectLink == null)
                return NotFound();

            await _linkRedirectRepository.SaveRedirect(HttpContext.GetIp(), redirectLink.AffiliateId, key, _setting.IpCacheTime);
            
            return Redirect(redirectLink.RedirectUrl);
        }
    }
}
