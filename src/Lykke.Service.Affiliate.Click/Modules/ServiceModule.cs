using Autofac;
using Common.Log;
using Lykke.Sdk;
using Lykke.Service.Affiliate.Click.Settings.ServiceSettings;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Repositories;
using Lykke.Service.Affiliate.Services.Managers;
using Lykke.SettingsReader;
using MongoDB.Driver;

namespace Lykke.Service.Affiliate.Click.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AffiliateClickSettings> _settings;

        public ServiceModule(IReloadingManager<AffiliateClickSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.CurrentValue.RedirectIpCacheSetting);

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            BuildRepositories(builder);
        }

        private void BuildRepositories(ContainerBuilder builder)
        {
            var mongoClient = new MongoClient(_settings.ConnectionString(x => x.Db.MongoConnString).CurrentValue);
            
            builder.RegisterInstance(new LinkRepository(new MongoStorage<LinkEntity>(mongoClient, "Links"))).As<ILinkRepository>();

            builder.RegisterInstance(new LinkRedirectRepository(new MongoStorage<LinkRedirectEntity>(mongoClient, "LinkRedirects"))).As<ILinkRedirectRepository>();
        }
    }
}
