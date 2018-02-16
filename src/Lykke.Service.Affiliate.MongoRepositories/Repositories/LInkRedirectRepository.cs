using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class LinkRedirectEntity : MongoEntity, ILinkRedirect
    {
        [BsonIgnore]
        public string Ip => BsonId;
        public string LinkId { get; set; }
        public string AffiliateId { get; set; }

        public static LinkRedirectEntity Create(string ip, string affiliateId, string linkId)
        {
            return new LinkRedirectEntity
            {
                AffiliateId = affiliateId,
                BsonId = ip,
                LinkId = linkId
            };
        }
    }

    public class LinkRedirectRepository : ILinkRedirectRepository
    {
        private readonly IMongoStorage<LinkRedirectEntity> _table;

        public LinkRedirectRepository(IMongoStorage<LinkRedirectEntity> table)
        {
            _table = table;
        }

        public Task SaveRedirect(string ip, string affiliateId, string linkId, TimeSpan ipCache)
        {
            return _table.InsertOrModifyAsync(ip, () => LinkRedirectEntity.Create(ip, affiliateId, linkId), (entity) =>
            {
                if (DateTime.UtcNow - entity.BsonCreateDt < ipCache)
                    return null;

                return LinkRedirectEntity.Create(ip, affiliateId, linkId);
            });
        }

        public async Task<ILinkRedirect> GetRedirect(string ip)
        {
            return await _table.GetDataAsync(ip);
        }
    }
}
