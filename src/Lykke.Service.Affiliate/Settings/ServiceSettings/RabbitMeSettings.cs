using System;
using System.Collections.Generic;
using System.Text;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class RabbitMeSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string ExchangeTrade { get; set; }
        public string ExchangeLimit { get; set; }

        public string QueueTrade { get; set; }
        public string QueueLimit { get; set; }
    }
}
