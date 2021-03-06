﻿using System;
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
        private readonly IReferralService _referralService;
        private readonly IClientAccountService _clientAccountService;
        private readonly IAccrualService _accrualService;
        private readonly IAffiliateService _affiliateService;
        private readonly IMapper _mapper;

        public AffiliateController(
            IReferralService referralService, 
            IMapper mapper, 
            ILinkService linkService, 
            IClientAccountService clientAccountService, 
            IAccrualService accrualService,
            IAffiliateService affiliateService)
        {
            _referralService = referralService;
            _mapper = mapper;
            _linkService = linkService;
            _clientAccountService = clientAccountService;
            _accrualService = accrualService;
            _affiliateService = affiliateService;
        }

        [HttpGet("count")]
        [SwaggerOperation("GetAffiliatesCount")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<int> GetAffiliatesCount()
        {
            var count = (await _affiliateService.GetAllAffiliates()).Count();
            return count;
        }
        
        [HttpGet("referrals")]
        [SwaggerOperation("GetReferrals")]
        [ProducesResponseType(typeof(IEnumerable<ReferralModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<ReferralModel>> GetReferrals([FromQuery]string clientId)
        {
            var referrals = await _referralService.GetReferralsAsync(clientId);

            var result = _mapper.Map<IEnumerable<IReferral>, IEnumerable<ReferralModel>>(referrals);

            return result;
        }
        
        [HttpGet("referrals/count")]
        [SwaggerOperation("GetReferralsCount")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<int> GetReferralsCount()
        {
            var count = await _referralService.GetReferralsCountAsync();
            return count;
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

        [HttpGet("stats")]
        [SwaggerOperation("GetStats")]
        [ProducesResponseType(typeof(IEnumerable<StatisticItemModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<StatisticItemModel>> GetStats([FromQuery] string clientId)
        {
            var stats = await _accrualService.GetStats(clientId);

            var result = _mapper.Map<IEnumerable<StatisticsItem>, IEnumerable<StatisticItemModel>>(stats);

            return result;
        }

        private async Task<bool> ValidateClientId(string clientId)
        {
            var client = await _clientAccountService.GetClientByIdAsync(clientId);

            return client != null;
        }
    }
}
