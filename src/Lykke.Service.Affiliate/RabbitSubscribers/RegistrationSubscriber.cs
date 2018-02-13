using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.RabbitSubscribers.Contracts;
using Lykke.Service.Affiliate.Settings.ServiceSettings;

namespace Lykke.Service.Affiliate.RabbitSubscribers
{
    public class RegistrationSubscriber : IQueueSubscriber
    {
        private RabbitMqSubscriber<ClientAuthInfo> _subscriber;
        private readonly ILog _log;
        private readonly RabbitRegistrationSettings _rabbitSettings;
        private readonly IAffiliateService _affiliateService;

        public RegistrationSubscriber(ILog log, RabbitRegistrationSettings rabbitSettings, IAffiliateService affiliateService)
        {
            _log = log;
            _rabbitSettings = rabbitSettings;
            _affiliateService = affiliateService;
        }

        public void Start()
        {
            // NOTE: Read https://github.com/LykkeCity/Lykke.RabbitMqDotNetBroker/blob/master/README.md to learn
            // about RabbitMq subscriber configuration

            var settings = RabbitMqSubscriptionSettings.CreateForSubscriber(
                _rabbitSettings.ConnectionString,
                _rabbitSettings.Exchange,
                _rabbitSettings.Queue);

            settings.MakeDurable();

            _subscriber = new RabbitMqSubscriber<ClientAuthInfo>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        retryTimeout: TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ClientAuthInfo>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();
        }

        private async Task ProcessMessageAsync(ClientAuthInfo model)
        {
            await _affiliateService.Register(model.Ip, model.ClientId);
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }
    }
}
