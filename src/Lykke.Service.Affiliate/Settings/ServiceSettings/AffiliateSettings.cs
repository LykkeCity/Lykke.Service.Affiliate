﻿namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class AffiliateSettings
    {
        public DbSettings Db { get; set; }

        public RabbitRegistrationSettings RabbitRegistration { get; set; }
        
        public RabbitMeSettings RabbitMe { get; set; }
    }
}
