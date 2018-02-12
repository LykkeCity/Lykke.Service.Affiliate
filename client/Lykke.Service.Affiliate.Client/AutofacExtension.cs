using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.Affiliate.Client
{
    public static class AutofacExtension
    {
        public static void RegisterAffiliateClient(this ContainerBuilder builder, string serviceUrl, ILog log)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<AffiliateClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IAffiliateClient>()
                .SingleInstance();
        }

        public static void RegisterAffiliateClient(this ContainerBuilder builder, AffiliateServiceClientSettings settings, ILog log)
        {
            builder.RegisterAffiliateClient(settings?.ServiceUrl, log);
        }
    }
}
