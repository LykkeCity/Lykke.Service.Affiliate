using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class AccrualPeriodProcesor : IAccrualPeriodProcesor
    {
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateProcessor _affiliateProcessor;

        public AccrualPeriodProcesor(IAffiliateService affiliateService, IAffiliateProcessor affiliateProcessor)
        {
            _affiliateService = affiliateService;
            _affiliateProcessor = affiliateProcessor;
        }

        public async Task<AccrualPeriodProcessorResult> Process(DateTime startDt, DateTime endDt)
        {
            var affiliates = await _affiliateService.GetAllAffiliates();

            var processedCount = 0;

            foreach (var affiliate in affiliates)
            {
                var result = await _affiliateProcessor.Process(affiliate, startDt, endDt);
                if (result.HasTrades)
                    processedCount++;
            }

            return new AccrualPeriodProcessorResult
            {
                AffiliateCount = processedCount
            };
        }
    }
}
