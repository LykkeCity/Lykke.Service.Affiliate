using Lykke.Service.Affiliate.Click.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Click.Settings.SlackNotifications;

namespace Lykke.Service.Affiliate.Click.Settings
{
    public class AppSettings
    {
        public AffiliateClickSettings AffiliateClickService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
