using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Affiliate.Settings.ServiceSettings
{
    public class CqrsSettings
    {
        [AmqpCheck]
        public string RabbitConnString { get; set; }
    }
}
