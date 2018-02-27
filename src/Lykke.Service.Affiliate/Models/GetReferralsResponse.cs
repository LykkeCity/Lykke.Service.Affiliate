using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.Affiliate.Contracts;

namespace Lykke.Service.Affiliate.Models
{
    public class GetReferralsResponse
    {
        public IEnumerable<ReferralModel> Referrals { get; set; }
    }
}
