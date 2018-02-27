using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Core
{
    public class Constants
    {
        public const string PaidFeeQueueName = "paid-fee-queue";

        public static string GetCacheReferralKey(string referralId)
        {
            return $"Referral_{referralId}";
        }

        public static string GetCacheDisabledAssetKey(string assetId)
        {
            return $"DisabledAsset_{assetId}";
        }
    }
}
