﻿using MongoDB.Driver;

namespace Lykke.Service.Affiliate.MongoRepositories.Utils
{
	public static class MongoExceptionExtensions
	{
		public static bool IsDuplicateError(this MongoServerException ex)
		{
			return ex.Message.Contains("duplicate key error");
		}
	}
}
