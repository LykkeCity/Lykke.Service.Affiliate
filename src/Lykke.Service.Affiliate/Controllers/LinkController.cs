using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;

        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost]
        [SwaggerOperation("RegisterLink")]
        public async Task<IActionResult> RegisterLink([FromBody]RegisterLinkModel model)
        {
            if (string.IsNullOrWhiteSpace(model.PartnerId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(model.RedirectUrl) || !Uri.IsWellFormedUriString(model.RedirectUrl, UriKind.Absolute))
                return BadRequest();

            var link = await _linkService.CreateNewLink(model.PartnerId, model.RedirectUrl);

            return Ok(new RegisterLinkResponse
            {
                Url = link
            });
        }

        [HttpGet]
        [SwaggerOperation("Links")]
        public async Task<IActionResult> GetLinks([FromQuery]string clientId)
        {
            var links = await _linkService.GetLinks(clientId);

            return Ok(new GetLinksResponse
            {
                Links = links.Select(x => new GetLinkResponse
                {
                    RedirectUrl = x.RedirectUrl,
                    Url = x.Url
                })
            });
        }
    }
}
