using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lykke.Service.Affiliate.AzureRepositories.Mongo
{	
	public abstract class MongoEntity
	{
		[BsonId]
		public string BsonId { get; set; }

        [BsonElement("_version")]
		public int BsonVersion { get; set; }

        [BsonElement("_created")]
        public DateTime BsonCreateDt { get; set; }

	    [BsonElement("_updated")]
        public DateTime? BsonUpdateDt { get; set; }

        [BsonElement("_batchId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? BatchId { get; set; }

	    protected MongoEntity()
	    {
	        BsonCreateDt = DateTime.UtcNow;
	    }
	}
}
