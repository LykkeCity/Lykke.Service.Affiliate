using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
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
        private readonly ILog _log;

        public BonusProcessor(TimeSpan period, TimeSpan periodOffset, IAccrualPeriodRepository accrualPeriodRepository, IAccrualPeriodProcesor periodProcessor, IAffiliateService affiliateService, ILogFactory logFactory)
        {
            _accrualPeriodRepository = accrualPeriodRepository;
            _periodOffset = periodOffset;
            _periodProcessor = periodProcessor;
            _affiliateService = affiliateService;
            _log = logFactory.CreateLog(this);
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
                    _log.Error(nameof(Process), ex, $"Affiliate: {affiliate}");
                }
            }
        }

        private async Task ProcessOneUser(string affiliateId)
        {
            var period = await _accrualPeriodRepository.GetLastPeriod(affiliateId);

            if (period == null)
            {
                // set initial period as previous one
                period = await GetNewPeriod(affiliateId, DateTime.UtcNow.Date + _periodOffset - _period);
            }
            else if (period.Completed)
            {
                period = await GetNewPeriod(affiliateId, period.EndDt);
            }

            if (period == null)
                return;

            do
            {
                _log.Info(nameof(ProcessOneUser), "start processing period", $"AffiliateId: {affiliateId}, period: {period.Id}");

                await _periodProcessor.Process(period);

                await _accrualPeriodRepository.SetCompleted(period.Id);

                _log.Info(nameof(ProcessOneUser), "finish processing period", $"AffiliateId: {affiliateId}, period: {period.Id}");

                period = await GetNewPeriod(affiliateId, period.EndDt);

                await Task.Delay(100);

            } while (period != null);
        }

        private async Task<IAccrualPeriod> GetNewPeriod(string affiliateId, DateTime startDt)
        {
            var newEnd = startDt + _period;

            // new period no finished yet
            if (newEnd > DateTime.UtcNow)
                return null;

            return await _accrualPeriodRepository.CreatePeriod(affiliateId, startDt, newEnd);
        }
    }
}
