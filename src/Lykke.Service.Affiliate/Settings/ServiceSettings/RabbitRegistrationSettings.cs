using System;
using System.Collections.Generic;
using System.Text;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class RabbitRegistrationSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }
    }
}
