using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class BonusProcessor : IBonusProcessor
    {
        private readonly IAccrualPeriodRepository _accrualPeriodRepository;
        private readonly TimeSpan _period;
        private readonly TimeSpan _periodOffset;
        private readonly IAccrualPeriodProcesor _periodProcessor;

        public BonusProcessor(IAccrualPeriodRepository accrualPeriodRepository, TimeSpan period, TimeSpan periodOffset, IAccrualPeriodProcesor periodProcessor)
        {
            _accrualPeriodRepository = accrualPeriodRepository;
            _periodOffset = periodOffset;
            _periodProcessor = periodProcessor;
            _period = period;
        }

        public async Task Process()
        {
            var lastEndDt = await CalcLastEndDate();
            if (lastEndDt == null)
                return;

            var startDt = lastEndDt.Value;
            var endDt = startDt + _period;
            
            var result = await _periodProcessor.Process(startDt, endDt);
            await _accrualPeriodRepository.CreatePeriod(startDt, endDt, result.AffiliateCount);
        }


        private async Task<DateTime?> CalcLastEndDate()
        {
            var period = await _accrualPeriodRepository.GetLastPeriod();
            if (period != null)
                return period.EndDt;

            var newStart = DateTime.UtcNow.Date + _periodOffset;
            var newEnd = newStart + _period;

            // new period is not finished
            if (newEnd > DateTime.UtcNow)
                return null;

            return newStart;
        }
    }
}
