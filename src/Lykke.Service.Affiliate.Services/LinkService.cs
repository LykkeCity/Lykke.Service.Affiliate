using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services;

namespace Lykke.Service.Affiliate.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly Uri _affiliateClickUri;

        public LinkService(ILinkRepository linkRepository, string affiliateClickUrl)
        {
            _linkRepository = linkRepository;
            _affiliateClickUri = new Uri(affiliateClickUrl);
        }

        public async Task<LinkResult> CreateNewLink(string clientId, string redirectUrl)
        {
            var link = await _linkRepository.CreateAsync(clientId, redirectUrl);

            return new LinkResult { Url = GetLinkUrl(link.Key), RedirectUrl = redirectUrl };
        }

        public async Task<IEnumerable<LinkResult>> GetLinks(string clientId)
        {
            var links = await _linkRepository.GetLinks(clientId);

            return links.Select(x => new LinkResult() { Url = GetLinkUrl(x.Key), RedirectUrl = x.RedirectUrl });
        }

        private string GetLinkUrl(string key)
        {
            var uri = new Uri(_affiliateClickUri, key);

            return uri.ToString();
        }
    }
}
