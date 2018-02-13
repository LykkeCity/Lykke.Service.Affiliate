using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.AzureRepositories.Mongo;
using Lykke.Service.Affiliate.Core.Domain.Repositories;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.AzureRepositories.Repositories
{
    public class ReferralEntity : MongoEntity, IReferralEntity
    {
        [BsonIgnore]
        public string ReferralId => BsonId;

        public string AffiliateId { get; set; }

        public static ReferralEntity Create(string affiliateId, string referralId)
        {
            return new ReferralEntity
            {
                BsonId = referralId,
                AffiliateId = affiliateId
            };
        }
    }

    public class ReferralRepository : IReferralRepository
    {
        private readonly IMongoStorage<ReferralEntity> _table;

        public ReferralRepository(IMongoStorage<ReferralEntity> table)
        {
            _table = table;
        }

        public Task SaveReferral(string clientId, string affiliateId)
        {
            return _table.InsertOrModifyAsync(clientId, () => ReferralEntity.Create(affiliateId, clientId), (entity) => entity);
        }
    }
}
