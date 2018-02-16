using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class AffiliateProcessor : IAffiliateProcessor
    {
        public Task<AffiliateProcessorResult> Process(string affiliateId, DateTime startDt, DateTime endDt)
        {
            return Task.FromResult(new AffiliateProcessorResult());
        }
    }
}
