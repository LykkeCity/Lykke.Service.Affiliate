using Lykke.Service.Affiliate.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Settings.SlackNotifications;

namespace Lykke.Service.Affiliate.Settings
{
    public class AppSettings
    {
        public AffiliateClickSettings AffiliateClickService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
