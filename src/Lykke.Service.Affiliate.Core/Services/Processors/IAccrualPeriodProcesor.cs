using System;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.Core.Services.Processors
{
    public interface IAccrualPeriodProcesor
    {
        Task Process(IAccrualPeriod period);
    }
}
