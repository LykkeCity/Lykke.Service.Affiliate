using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.ExchangeOperations.Client.AutorestClient.Models;

namespace Lykke.Service.Affiliate.Services
{
    public static class ExchangeOperationsExtensions
    {
        private const int MeDupliacateCode = 430;

        public static bool IsDuplicated(this ExchangeOperationResult result)
        {
            return result?.Code == MeDupliacateCode;
        }
    }
}
