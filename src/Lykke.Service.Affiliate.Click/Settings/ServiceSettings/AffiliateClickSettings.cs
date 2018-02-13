using System;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class AffiliateClickSettings
    {
        public DbSettings Db { get; set; }

        public RedirectIpCacheSetting RedirectIpCacheSetting { get; set; }
    }

    public class RedirectIpCacheSetting
    {
        public TimeSpan IpCacheTime { get; set; }
    }
}
