using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Affiliate.Click.Settings.ServiceSettings;

namespace Lykke.Service.Affiliate.Click.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public AffiliateClickSettings AffiliateClickService { get; set; }
    }
}
