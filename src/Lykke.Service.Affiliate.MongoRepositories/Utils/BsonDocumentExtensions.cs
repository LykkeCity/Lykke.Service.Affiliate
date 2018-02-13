using MongoDB.Bson;

namespace Lykke.Service.Affiliate.AzureRepositories.Utils
{
	public static class BsonDocumentExtensions
	{
		public static BsonDocument MergeExt(this BsonDocument doc, BsonDocument doc2)
		{
			foreach (var bsonElement in doc2.Elements)
			{
				if (bsonElement.Value != BsonNull.Value)
					doc[bsonElement.Name] = bsonElement.Value;
			}
			return doc;
		}
	}
}
