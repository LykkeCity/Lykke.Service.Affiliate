using System.Threading.Tasks;
using Lykke.JobTriggers.Triggers.Attributes;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Triggers
{
    public class PaidFeeFunction
    {
        private readonly IPaidFeeProcessor _paidFeeProcessor;

        public PaidFeeFunction(IPaidFeeProcessor paidFeeProcessor)
        {
            _paidFeeProcessor = paidFeeProcessor;
        }

        [QueueTrigger(Constants.PaidFeeQueueName, maxPollingIntervalMs: 100)]
        public async Task Process(PaidFeeQueueItem item)
        {
            await _paidFeeProcessor.Process(item);
        }
    }
}
