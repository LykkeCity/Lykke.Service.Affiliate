using System;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services.Processors
{
    public class AffiliateProcessorResult
    {
        public bool HasTrades { get; set; }
    }

    public interface IAffiliateProcessor
    {
        Task<AffiliateProcessorResult> Process(string affiliateId, DateTime startDt, DateTime endDt);
    }
}
