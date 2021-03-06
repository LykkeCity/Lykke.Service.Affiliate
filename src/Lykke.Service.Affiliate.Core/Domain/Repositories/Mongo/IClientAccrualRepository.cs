﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo
{
    public interface IClientAccrual
    {
        string Id { get; }
        string AccrualPeriodId { get; }
        string MeId { get; }
        string ClientId { get; }
        string AssetId { get; }
        decimal TradeVolume { get; }
        decimal Bonus { get; }
        bool Completed { get; }
    }

    public interface IClientAccrualRepository
    {
        Task<IClientAccrual> Create(string accrualPeriodId, string clientId, string assetId, decimal tradeVolume, decimal bonus);
        Task<IClientAccrual> GetClientAccrual(string accrualPeriodId, string asset);
        Task SetCompleted(string id);
        Task<IEnumerable<IClientAccrual>> GetClientAccruals(string clientId);
        Task<IEnumerable<IClientAccrual>> GetClientAccruals();
    }
}
