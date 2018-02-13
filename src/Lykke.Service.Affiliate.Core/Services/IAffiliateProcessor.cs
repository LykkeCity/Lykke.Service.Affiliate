using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface IAffiliateProcessor
    {
        Task Process(string affiliateId, DateTime startDt, DateTime endDt);
    }
}
