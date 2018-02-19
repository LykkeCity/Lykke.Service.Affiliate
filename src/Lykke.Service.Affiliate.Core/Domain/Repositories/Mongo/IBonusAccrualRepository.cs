﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IBonusAccrual
    {
        string ClientId { get; set; }
        string AssetId { get; set; }
        decimal Amount { get; set; }
        string PaidFeeId { get; set; }
        DateTime CreatedDt { get; }
    }

    public interface IBonusAccrualRepository
    {
        Task Create(string paidFeeId, string clientId, string assetId, decimal amount);
        Task<IEnumerable<IBonusAccrual>> GetData(string affiliateId, DateTime startDt, DateTime endDt);
    }
}
