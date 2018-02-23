using Lykke.Service.Affiliate.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Settings.SlackNotifications;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ExchangeOperations.Client;

namespace Lykke.Service.Affiliate.Settings
{
    public class AppSettings
    {
        public AffiliateSettings AffiliateService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; }
        public ExchangeOperationsServiceClientSettings ExchangeOperationsServiceClient { get; set; }
        public FeeSettings FeeSettings { get; set; }
    }
}
