using System;
using System.Threading.Tasks;
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
        public DateTime ExpirationDt { get; set; }
        public string AffiliateId { get; set; }

        public static LinkRedirectEntity Create(string ip, string affiliateId, string linkId, DateTime expirationDt)
        {
            return new LinkRedirectEntity
            {
                AffiliateId = affiliateId,
                BsonId = ip,
                LinkId = linkId,
                ExpirationDt = expirationDt
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
            var expirationDt = DateTime.UtcNow + ipCache;
            return _table.InsertOrModifyAsync(ip, () => LinkRedirectEntity.Create(ip, affiliateId, linkId, expirationDt), (entity) =>
            {
                if (DateTime.UtcNow < entity.ExpirationDt)
                    return null;

                entity.AffiliateId = affiliateId;
                entity.LinkId = linkId;
                entity.ExpirationDt = expirationDt;

                return entity;
            });
        }

        public async Task<ILinkRedirect> GetRedirect(string ip)
        {
            return await _table.GetDataAsync(ip);
        }
    }
}
