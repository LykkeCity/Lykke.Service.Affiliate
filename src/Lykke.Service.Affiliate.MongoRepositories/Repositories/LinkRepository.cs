using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.AzureRepositories.Repositories
{
    public class LinkEntity : MongoEntity, ILink
    {
        public string Key => BsonId;

        public string AffiliateId { get; set; }

        public string RedirectUrl { get; set; }

        public static LinkEntity Create(string affiliateId, string redirectUrl)
        {
            return new LinkEntity
            {
                BsonId = Guid.NewGuid().ToString("n"),
                AffiliateId = affiliateId,
                RedirectUrl = redirectUrl
            };
        }
    }

    public class LinkRepository : ILinkRepository
    {
        private readonly IMongoStorage<LinkEntity> _table;

        public LinkRepository(IMongoStorage<LinkEntity> table)
        {
            _table = table;
        }

        public async Task<ILink> CreateAsync(string affiliateId, string redirectUrl)
        {
            var link = LinkEntity.Create(affiliateId, redirectUrl);

            await _table.InsertAsync(link);

            return link;
        }

        public async Task<ILink> GetAsync(string key)
        {
            return await _table.GetDataAsync(key);
        }

        public async Task<IEnumerable<ILink>> GetLinks(string clientId)
        {
            return await _table.GetDataAsync(x => x.AffiliateId == clientId);
        }
    }
}
