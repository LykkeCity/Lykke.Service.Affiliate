using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.Models
{
    public class GetLinksResponse
    {
        public IEnumerable<GetLinkResponse> Links { get; set; }
    }

    public class GetLinkResponse
    {
        public string Url { get; set; }
        public string RedirectUrl { get; set; }
    }
}
