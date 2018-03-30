using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Affiliate.Contracts;
using Lykke.Service.Affiliate.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Affiliate.Controllers
{
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly ISummaryStatsService _summaryStatsService;
        private readonly IMapper _mapper;

        public StatsController(
            ISummaryStatsService summaryStatsService,
            IMapper mapper
            )
        {
            _summaryStatsService = summaryStatsService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [SwaggerOperation("GetSummaryStats")]
        [ProducesResponseType(typeof(IEnumerable<StatisticItemModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<StatisticItemModel>> GetSummaryStats()
        {
            var data = await _summaryStatsService.GetSummaryStatsAsync();
            var result = _mapper.Map<IEnumerable<StatisticsItem>, IEnumerable<StatisticItemModel>>(data);

            return result;
        }
        
        [HttpGet("period")]
        [SwaggerOperation("GetSummaryStatsForPeriod")]
        [ProducesResponseType(typeof(IEnumerable<StatisticItemModel>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<StatisticItemModel>> GetSummaryStatsForPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var data = await _summaryStatsService.GetSummaryStatsAsync(startDate, endDate);
            var result = _mapper.Map<IEnumerable<StatisticsItem>, IEnumerable<StatisticItemModel>>(data);

            return result;
        }
    }
}
