using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IAccrualPeriod
    {
        DateTime StartDt { get; set; }
        DateTime EndDt { get; set; }
        int AffiliateCount { get; set; }
    }

    public interface IAccrualPeriodRepository
    {
        Task<IAccrualPeriod> GetLastPeriod();

        Task CreatePeriod(DateTime startDt, DateTime endDt, int affiliateCount);
    }
}
