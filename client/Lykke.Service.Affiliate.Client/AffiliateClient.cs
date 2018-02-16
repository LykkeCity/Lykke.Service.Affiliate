using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Service.Affiliate.Client.AutorestClient;
using Lykke.Service.Affiliate.Contracts;
using Microsoft.Rest;

namespace Lykke.Service.Affiliate.Client
{
    public class AffiliateClient : IAffiliateClient, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly IAffiliateAPI _affiliateApi;

        public AffiliateClient(string serviceUrl)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ClientAutomapperProfile>();
            });

            _mapper = config.CreateMapper();

            _affiliateApi = new AffiliateAPI(new Uri(serviceUrl));
        }

        public async Task<LinkModel> RegisterLink(string partnerId, string redirectUrl)
        {
            var result = await _affiliateApi.RegisterLinkAsync(new AutorestClient.Models.RegisterLinkModel
            {
                PartnerId = partnerId,
                RedirectUrl = redirectUrl
            });

            var output = _mapper.Map<LinkModel>(result);

            return output;
        }

        public async Task<IEnumerable<LinkModel>> GetLinks(string partnerId)
        {
            var result = await _affiliateApi.LinksAsync(partnerId);

            var output = _mapper.Map<IEnumerable<LinkModel>>(result);

            return output;
        }

        public async Task<IEnumerable<ReferralModel>> GetReferrals(string partnerId)
        {
            var result = await _affiliateApi.GetReferralsAsync(partnerId);

            var output = _mapper.Map<IEnumerable<ReferralModel>>(result);

            return output;
        }

        public void Dispose()
        {
            _affiliateApi?.Dispose();
        }
    }
}
