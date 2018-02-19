using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IAccrualPeriod
    {
        string Id { get; }
        string ClientId { get; set; }
        DateTime StartDt { get; set; }
        DateTime EndDt { get; set; }
        bool Completed { get; set; }
    }

    public interface IAccrualPeriodRepository
    {
        Task<IAccrualPeriod> GetLastPeriod(string clientId);

        Task<IAccrualPeriod> CreatePeriod(string clientId, DateTime startDt, DateTime endDt);

        Task SetCompleted(string accrualPeriodId);
    }
}
