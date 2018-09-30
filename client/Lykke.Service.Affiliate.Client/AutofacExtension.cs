using System;
using Autofac;

namespace Lykke.Service.Affiliate.Client
{
    public static class AutofacExtension
    {
        public static void RegisterAffiliateClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<AffiliateClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IAffiliateClient>()
                .SingleInstance();
        }

        public static void RegisterAffiliateClient(this ContainerBuilder builder, AffiliateServiceClientSettings settings)
        {
            builder.RegisterAffiliateClient(settings?.ServiceUrl);
        }
    }
}
