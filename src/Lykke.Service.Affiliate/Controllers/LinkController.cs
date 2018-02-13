using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class LinkController : Controller
    {
        private readonly ILinkRepository _linkRepository;

        public LinkController(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        [HttpPost]
        [SwaggerOperation("RegisterLink")]
        public async Task<IActionResult> RegisterLink([FromBody]RegisterLinkModel model)
        {
            var link = await _linkRepository.CreateAsync(model.PartnerId, model.RedirectUrl);

            return Ok(new RegisterLinkResponse
            {
                Url = link.Key
            });
        }
    }
}
