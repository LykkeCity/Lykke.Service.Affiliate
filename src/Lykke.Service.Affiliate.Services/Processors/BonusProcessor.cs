using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class BonusProcessor : IBonusProcessor
    {
        private readonly IAccrualPeriodRepository _accrualPeriodRepository;
        private readonly TimeSpan _period;
        private readonly TimeSpan _periodOffset;
        private readonly IAccrualPeriodProcesor _periodProcessor;
        private readonly IAffiliateService _affiliateService;
        private readonly ILog _logger;

        public BonusProcessor(TimeSpan period, TimeSpan periodOffset, IAccrualPeriodRepository accrualPeriodRepository, IAccrualPeriodProcesor periodProcessor, IAffiliateService affiliateService, ILog logger)
        {
            _accrualPeriodRepository = accrualPeriodRepository;
            _periodOffset = periodOffset;
            _periodProcessor = periodProcessor;
            _affiliateService = affiliateService;
            _logger = logger;
            _period = period;
        }

        public async Task Process()
        {
            var affiliates = await _affiliateService.GetAllAffiliates();
            
            foreach (var affiliate in affiliates)
            {
                try
                {
                    await ProcessOneUser(affiliate);
                }
                catch (Exception ex)
                {
                    await _logger.WriteErrorAsync(nameof(BonusProcessor), nameof(Process), $"Affilaite: {affiliate}", ex);
                }
            }
        }

        public async Task ProcessOneUser(string affiliateId)
        {
            var lastEndDt = await CalcLastEndDate(affiliateId);
            if (lastEndDt == null)
                return;

            var startDt = lastEndDt.Value;
            var endDt = startDt + _period;

            var period = await _accrualPeriodRepository.CreatePeriod(affiliateId, startDt, endDt);

            await _periodProcessor.Process(period);

            await _accrualPeriodRepository.SetCompleted(period.Id);
        }


        private async Task<DateTime?> CalcLastEndDate(string affiliateId)
        {
            var period = await _accrualPeriodRepository.GetLastPeriod(affiliateId);
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
