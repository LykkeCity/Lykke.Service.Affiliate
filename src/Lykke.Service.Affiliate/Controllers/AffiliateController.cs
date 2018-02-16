using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Affiliate.Contracts;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class AffiliateController : Controller
    {
        private readonly ILinkService _linkService;
        private readonly IReferralRepository _referralRepository;
        private readonly IMapper _mapper;

        public AffiliateController(IReferralRepository referralRepository, IMapper mapper, ILinkService linkService)
        {
            _referralRepository = referralRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet("referrals")]
        [SwaggerOperation("GetReferrals")]
        [ProducesResponseType(typeof(IEnumerable<ReferralModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<ReferralModel>> GetReferrals([FromQuery]string partnerId)
        {
            var referrals = await _referralRepository.GetReferrals(partnerId);

            var result = _mapper.Map<IEnumerable<IReferral>, IEnumerable<ReferralModel>>(referrals);

            return result;
        }

        [HttpGet("links")]
        [SwaggerOperation("GetLinks")]
        [ProducesResponseType(typeof(IEnumerable<LinkModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<LinkModel>> GetLinks([FromQuery]string partnerId)
        {
            var links = await _linkService.GetLinks(partnerId);

            var result = _mapper.Map<IEnumerable<LinkResult>, IEnumerable<LinkModel>>(links);

            return result;
        }

        [HttpPost("link")]
        [SwaggerOperation("RegisterLink")]
        [ProducesResponseType(typeof(LinkModel), (int)HttpStatusCode.OK)]
        public async Task<LinkModel> RegisterLink([FromBody]RegisterLinkModel model)
        {
            if (string.IsNullOrWhiteSpace(model.PartnerId))
                throw new ApiException(HttpStatusCode.BadRequest, "Partner id must be not empty string");

            if (string.IsNullOrWhiteSpace(model.RedirectUrl) || !Uri.IsWellFormedUriString(model.RedirectUrl, UriKind.Absolute))
                throw new ApiException(HttpStatusCode.BadRequest, "Redirect URL must be a valid URL string");

            var link = await _linkService.CreateNewLink(model.PartnerId, model.RedirectUrl);

            var result = _mapper.Map<LinkResult, LinkModel>(link);

            return result;
        }
    }
}
