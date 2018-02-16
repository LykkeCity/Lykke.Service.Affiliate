using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Contracts
{
    public class ReferralModel
    {
        /// <summary>
        /// A ClientId of referral
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A time when referral was created
        /// </summary>
        public DateTime CreatedDt { get; set; }
    }
}
