using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Azure
{
    public class PaidFeeQueueItem
    {
        public Guid Id { get; set; }
        public string AssetId { get; set; }
        public string FromClient { get; set; }
        public string ToClient { get; set; }
        public decimal Volume { get; set; }
        public DateTime Date { get; set; }
        public string Order { get; set; }

    }

    public interface IPaidFeeQueueWriter
    {
        Task AddPaidFee(Guid id, string asset, string fromClient, string toClient, decimal volume, DateTime date, string order);
    }
}
