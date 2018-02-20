using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.MongoRepositories.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.MongoRepositories.Repositories
{
    public class Referral : MongoEntity, IReferral
    {
        [BsonIgnore]
        public string ReferralId => BsonId;

        public string AffiliateId { get; set; }

        [BsonIgnore]
        public DateTime CreatedDt => BsonCreateDt;

        public static Referral Create(string affiliateId, string referralId)
        {
            return new Referral
            {
                BsonId = referralId,
                AffiliateId = affiliateId
            };
        }
    }

    public class ReferralRepository : IReferralRepository
    {
        private readonly IMongoStorage<Referral> _table;

        public ReferralRepository(IMongoStorage<Referral> table)
        {
            _table = table;
        }

        public async Task<IReferral> SaveReferral(string clientId, string affiliateId)
        {
            return await _table.InsertOrModifyAsync(clientId, () => Referral.Create(affiliateId, clientId), (entity) => entity);
        }

        public async Task<IEnumerable<IReferral>> GetReferrals(string partnerId)
        {
            return await _table.GetDataAsync(x => x.AffiliateId == partnerId);
        }

        public async Task<IEnumerable<IReferral>> GetAllReferrals()
        {
            return await _table.GetDataAsync();
        }
    }
}
