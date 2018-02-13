using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.RabbitSubscribers.Contracts
{
    public class ClientAuthInfo
    {
        public string ClientId { get; set; }
        public string Ip { get; set; }
    }
}
