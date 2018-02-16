using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class AccrualPeriodSettings
    {
        public TimeSpan Period { get; set; }

        public TimeSpan PeriodOffset { get; set; }
    }
}
