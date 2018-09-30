using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Affiliate.Settings.ServiceSettings;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ExchangeOperations.Client;

namespace Lykke.Service.Affiliate.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public AffiliateSettings AffiliateService { get; set; }
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; }
        public ExchangeOperationsServiceClientSettings ExchangeOperationsServiceClient { get; set; }
        public FeeSettings FeeSettings { get; set; }
    }
}
