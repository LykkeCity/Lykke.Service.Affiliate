using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Affiliate.Contracts;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class AffiliateController : Controller
    {
        private readonly IReferralRepository _referralRepository;
        private readonly IMapper _mapper;

        public AffiliateController(IReferralRepository referralRepository, IMapper mapper)
        {
            _referralRepository = referralRepository;
            _mapper = mapper;
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

    }
}
