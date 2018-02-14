using Autofac;
using AzureStorage.Queue;
using Common.Log;
using Lykke.Service.Affiliate.AzureRepositories;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.AzureRepositories.Repositories;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.RabbitSubscribers;
using Lykke.Service.Affiliate.Services;
using Lykke.Service.Affiliate.Settings;
using Lykke.SettingsReader;
using MongoDB.Driver;

namespace Lykke.Service.Affiliate.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))
            
            builder.RegisterInstance(_settings.CurrentValue.AffiliateService.RabbitMe);
            builder.RegisterInstance(_settings.CurrentValue.AffiliateService.RabbitRegistration);

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            BuildRepositories(builder);
            BindQueue(builder);
            RegisterRabbitMqSubscribers(builder);

            builder.RegisterType<AffiliateService>().As<IAffiliateService>();

            builder.RegisterType<LinkService>().As<ILinkService>().WithParameter(TypedParameter.From(_settings.CurrentValue.AffiliateService.AffiliateClickUrl));
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<RegistrationSubscriber>().SingleInstance();
            builder.RegisterType<LimitTradeSubscriber>().SingleInstance();
            builder.RegisterType<TradeSubscriber>().SingleInstance();
        }


        private void BuildRepositories(ContainerBuilder builder)
        {
            var mongoClient = new MongoClient(_settings.ConnectionString(x => x.AffiliateService.Db.MongoConnString).CurrentValue);

            builder.RegisterInstance(new ReferralRepository(new MongoStorage<ReferralEntity>(mongoClient, "Referrals"))).As<IReferralRepository>();

            builder.RegisterInstance(new LinkRepository(new MongoStorage<LinkEntity>(mongoClient, "Links"))).As<ILinkRepository>();

            builder.RegisterInstance(new LinkRedirectRepository(new MongoStorage<LinkRedirectEntity>(mongoClient, "LinkRedirects"))).As<ILinkRedirectRepository>();
        }

        private void BindQueue(ContainerBuilder builder)
        {
            builder.RegisterInstance(new PaidFeeQueueWriter(
                AzureQueueExt.Create(_settings.ConnectionString(x => x.AffiliateService.Db.AzureConnString),
                    Constants.PaidFeeQueueName))).As<IPaidFeeQueueWriter>();
        }
    }
}
