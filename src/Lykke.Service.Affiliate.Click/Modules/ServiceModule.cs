using Autofac;
using Common.Log;
using Lykke.Service.Affiliate.Click.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Managers;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Repositories;
using Lykke.Service.Affiliate.Services;
using Lykke.Service.Affiliate.Services.Managers;
using Lykke.SettingsReader;
using MongoDB.Driver;

namespace Lykke.Service.Affiliate.Click.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AffiliateClickSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<AffiliateClickSettings> settings, ILog log)
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

            builder.RegisterInstance(_settings.CurrentValue.RedirectIpCacheSetting);

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
        }

        private void BuildRepositories(ContainerBuilder builder)
        {
            var connectionString = _settings.ConnectionString(x => x.Db.MongoConnString).CurrentValue;
            var mongoClient = new MongoClient(connectionString);
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;

            builder.RegisterInstance(new LinkRepository(new MongoStorage<LinkEntity>(mongoClient, "Links", databaseName))).As<ILinkRepository>();

            builder.RegisterInstance(new LinkRedirectRepository(new MongoStorage<LinkRedirectEntity>(mongoClient, "LinkRedirects", databaseName))).As<ILinkRedirectRepository>();
        }
    }
}
