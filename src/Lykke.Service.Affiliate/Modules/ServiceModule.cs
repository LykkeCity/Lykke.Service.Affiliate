using Autofac;
using AzureStorage.Queue;
using Common.Log;
using Lykke.JobTriggers.Extenstions;
using Lykke.Service.Affiliate.AzureRepositories;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Managers;
using Lykke.Service.Affiliate.Core.Services.Processors;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Repositories;
using Lykke.Service.Affiliate.RabbitSubscribers;
using Lykke.Service.Affiliate.Services;
using Lykke.Service.Affiliate.Services.Managers;
using Lykke.Service.Affiliate.Services.Processors;
using Lykke.Service.Affiliate.Settings;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ExchangeOperations.Client;
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
            var settings = _settings.CurrentValue;

            builder.RegisterInstance(settings.AffiliateService.RabbitMe);
            builder.RegisterInstance(settings.AffiliateService.RabbitRegistration);

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
            RegisterProcessors(builder);

            builder.RegisterType<AffiliateService>().As<IAffiliateService>();
            builder.RegisterType<ReferralService>().As<IReferralService>();
            builder.RegisterType<AccrualService>().As<IAccrualService>();
            builder.RegisterType<SummaryStatsService>().As<ISummaryStatsService>();

            builder.RegisterType<LinkService>().As<ILinkService>().WithParameter(TypedParameter.From(settings.AffiliateService.AffiliateClickUrl));

            builder.AddTriggers(pool =>
            {
                pool.AddDefaultConnection(_settings.ConnectionString(x => x.AffiliateService.Db.AzureConnString));
            });

            builder.RegisterLykkeServiceClient(settings.ClientAccountServiceClient.ServiceUrl);

            builder.RegisterInstance(new ExchangeOperationsServiceClient(settings.ExchangeOperationsServiceClient.ServiceUrl)).As<IExchangeOperationsServiceClient>().SingleInstance();
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<RegistrationSubscriber>().SingleInstance();
            builder.RegisterType<LimitTradeSubscriber>().SingleInstance();
            builder.RegisterType<TradeSubscriber>().SingleInstance();
        }

        private void RegisterProcessors(ContainerBuilder builder)
        {
            builder.RegisterType<AccrualPeriodProcesor>().As<IAccrualPeriodProcesor>()
                .WithParameter("feeClientId", _settings.CurrentValue.FeeSettings.TargetClientId.Affiliate);

            builder.RegisterType<BonusProcessor>().As<IBonusProcessor>()
                .WithParameter("period", _settings.CurrentValue.AffiliateService.AccrualPeriodSettings.Period)
                .WithParameter("periodOffset", _settings.CurrentValue.AffiliateService.AccrualPeriodSettings.PeriodOffset);

            builder.RegisterType<PaidFeeProcessor>().As<IPaidFeeProcessor>();
        }

        private void BuildRepositories(ContainerBuilder builder)
        {
            var mongoClient = new MongoClient(_settings.ConnectionString(x => x.AffiliateService.Db.MongoConnString).CurrentValue);

            builder.RegisterInstance(new ReferralRepository(new MongoStorage<Referral>(mongoClient, "Referrals"))).As<IReferralRepository>();

            builder.RegisterInstance(new LinkRepository(new MongoStorage<LinkEntity>(mongoClient, "Links"))).As<ILinkRepository>();

            builder.RegisterInstance(new LinkRedirectRepository(new MongoStorage<LinkRedirectEntity>(mongoClient, "LinkRedirects"))).As<ILinkRedirectRepository>();

            builder.RegisterInstance(new PaidFeeRepository(new MongoStorage<PaidFeeEntity>(mongoClient, "PaidFees"))).As<IPaidFeeRepository>();

            builder.RegisterInstance(new BonusAccrualRepository(new MongoStorage<BonusAccrualEntity>(mongoClient, "BonusAccruals"))).As<IBonusAccrualRepository>();

            builder.RegisterInstance(new ClientAccrualRepository(new MongoStorage<ClientAccrualEntity>(mongoClient, "ClientAccruals"))).As<IClientAccrualRepository>();

            builder.RegisterInstance(new AccrualPeriodRepository(new MongoStorage<AccrualPeriodEntity>(mongoClient, "AccrualPeriods"))).As<IAccrualPeriodRepository>();

            builder.RegisterInstance(new DisabledAssetRepository(new MongoStorage<DisabledAssetEntity>(mongoClient, "DisabledAssets"))).As<IDisabledAssetRepository>();
        }

        private void BindQueue(ContainerBuilder builder)
        {
            builder.RegisterInstance(new PaidFeeQueueWriter(
                AzureQueueExt.Create(_settings.ConnectionString(x => x.AffiliateService.Db.AzureConnString),
                    Constants.PaidFeeQueueName))).As<IPaidFeeQueueWriter>();
        }
    }
}
