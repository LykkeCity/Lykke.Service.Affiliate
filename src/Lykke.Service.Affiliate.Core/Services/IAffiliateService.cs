using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface IAffiliateService
    {
        Task Register(string ip, string clientId);

        Task<IEnumerable<string>> GetAllAffiliates();
    }
}
