using System;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services.Processors
{
    public class AccrualPeriodProcessorResult
    {
        public int AffiliateCount { get; set; }
    }

    public interface IAccrualPeriodProcesor
    {
        Task<AccrualPeriodProcessorResult> Process(DateTime startDt, DateTime endDt);
    }
}
