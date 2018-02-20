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
using Lykke.Service.ClientAccount.Client.AutorestClient;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class AffiliateController : Controller
    {
        private readonly ILinkService _linkService;
        private readonly IReferralRepository _referralRepository;
        private readonly IClientAccountService _clientAccountService;
        private readonly IMapper _mapper;

        public AffiliateController(IReferralRepository referralRepository, IMapper mapper, ILinkService linkService, IClientAccountService clientAccountService)
        {
            _referralRepository = referralRepository;
            _mapper = mapper;
            _linkService = linkService;
            _clientAccountService = clientAccountService;
        }

        [HttpGet("referrals")]
        [SwaggerOperation("GetReferrals")]
        [ProducesResponseType(typeof(IEnumerable<ReferralModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<ReferralModel>> GetReferrals([FromQuery]string clientId)
        {
            var referrals = await _referralRepository.GetReferrals(clientId);

            var result = _mapper.Map<IEnumerable<IReferral>, IEnumerable<ReferralModel>>(referrals);

            return result;
        }

        [HttpGet("links")]
        [SwaggerOperation("GetLinks")]
        [ProducesResponseType(typeof(IEnumerable<LinkModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<LinkModel>> GetLinks([FromQuery]string clientId)
        {
            var links = await _linkService.GetLinks(clientId);

            var result = _mapper.Map<IEnumerable<LinkResult>, IEnumerable<LinkModel>>(links);

            return result;
        }

        [HttpPost("link")]
        [SwaggerOperation("RegisterLink")]
        [ProducesResponseType(typeof(LinkModel), (int)HttpStatusCode.OK)]
        public async Task<LinkModel> RegisterLink([FromBody]RegisterLinkModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ClientId))
                throw new ApiException(HttpStatusCode.BadRequest, "Client id must be not empty string");

            if (!await ValidateClientId(model.ClientId))
                throw new ApiException(HttpStatusCode.BadRequest, "Client does not exist");

            if (string.IsNullOrWhiteSpace(model.RedirectUrl) || !Uri.IsWellFormedUriString(model.RedirectUrl, UriKind.Absolute))
                throw new ApiException(HttpStatusCode.BadRequest, "Redirect URL must be a valid URL string");

            var link = await _linkService.CreateNewLink(model.ClientId, model.RedirectUrl);

            var result = _mapper.Map<LinkResult, LinkModel>(link);

            return result;
        }

        private async Task<bool> ValidateClientId(string clientId)
        {
            var client = await _clientAccountService.GetClientByIdAsync(clientId);

            return client != null;
        }
    }
}
