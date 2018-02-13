using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class DbSettings
    {
        public string MongoConnString { get; set; }

        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
