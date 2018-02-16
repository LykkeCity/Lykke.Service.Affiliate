using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.JobTriggers.Triggers.Attributes;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Processors;

namespace Lykke.Service.Affiliate.Triggers
{

    public class BonusFunction
    {
        private readonly IBonusProcessor _bonusProcessor;

        public BonusFunction(IBonusProcessor bonusProcessor)
        {
            _bonusProcessor = bonusProcessor;
        }

        //[TimerTrigger("00:01:00")]
        public async Task Process()
        {
            await _bonusProcessor.Process();
        }
    }
}
