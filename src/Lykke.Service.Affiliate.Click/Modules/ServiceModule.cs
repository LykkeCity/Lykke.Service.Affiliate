using Autofac;
using Lykke.Service.Affiliate.Click.Settings;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Repositories;
using Lykke.SettingsReader;
using MongoDB.Driver;

namespace Lykke.Service.Affiliate.Click.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public ServiceModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.CurrentValue.AffiliateClickService.RedirectIpCacheSetting);

            BuildRepositories(builder);
        }

        private void BuildRepositories(ContainerBuilder builder)
        {
            var mongoClient = new MongoClient(_settings.ConnectionString(x => x.AffiliateClickService.Db.MongoConnString).CurrentValue);
            
            builder.RegisterInstance(new LinkRepository(new MongoStorage<LinkEntity>(mongoClient, "Links"))).As<ILinkRepository>();

            builder.RegisterInstance(new LinkRedirectRepository(new MongoStorage<LinkRedirectEntity>(mongoClient, "LinkRedirects"))).As<ILinkRedirectRepository>();
        }
    }
}
