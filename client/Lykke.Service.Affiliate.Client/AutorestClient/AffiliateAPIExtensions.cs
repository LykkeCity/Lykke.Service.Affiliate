// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.Affiliate.Client.AutorestClient
{
    using Lykke.Service;
    using Lykke.Service.Affiliate;
    using Lykke.Service.Affiliate.Client;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for AffiliateAPI.
    /// </summary>
    public static partial class AffiliateAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='partnerId'>
            /// </param>
            public static IList<ReferralModel> GetReferrals(this IAffiliateAPI operations, string partnerId = default(string))
            {
                return operations.GetReferralsAsync(partnerId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='partnerId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ReferralModel>> GetReferralsAsync(this IAffiliateAPI operations, string partnerId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetReferralsWithHttpMessagesAsync(partnerId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='partnerId'>
            /// </param>
            public static IList<LinkModel> GetLinks(this IAffiliateAPI operations, string partnerId = default(string))
            {
                return operations.GetLinksAsync(partnerId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='partnerId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<LinkModel>> GetLinksAsync(this IAffiliateAPI operations, string partnerId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLinksWithHttpMessagesAsync(partnerId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static LinkModel RegisterLink(this IAffiliateAPI operations, RegisterLinkModel model = default(RegisterLinkModel))
            {
                return operations.RegisterLinkAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<LinkModel> RegisterLinkAsync(this IAffiliateAPI operations, RegisterLinkModel model = default(RegisterLinkModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RegisterLinkWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object IsAlive(this IAffiliateAPI operations)
            {
                return operations.IsAliveAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsAliveAsync(this IAffiliateAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
