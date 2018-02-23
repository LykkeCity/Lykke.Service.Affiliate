using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class FeeSettings
    {
        public FeeTargetClientId TargetClientId { get; set; }

        public class FeeTargetClientId
        {
            public string Affiliate { get; set; }
        }
    }
}
