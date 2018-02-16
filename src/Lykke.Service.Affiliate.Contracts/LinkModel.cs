using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Contracts
{
    public class LinkModel
    {
        /// <summary>
        /// A partner link for referrals
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A redirect URL for partner link
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
