using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;

namespace Lykke.Service.Affiliate.Core.Services.Processors
{
    public interface IPaidFeeProcessor
    {
        Task Process(PaidFeeQueueItem item);
    }
}
