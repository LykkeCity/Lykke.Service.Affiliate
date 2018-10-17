using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services.Processors;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.ExchangeOperations.Client;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class AccrualPeriodProcesor : IAccrualPeriodProcesor
    {
        private readonly string _feeClientId;
        private readonly IClientAccrualRepository _clientAccrualRepository;
        private readonly IBonusAccrualRepository _bonusAccrualRepository;
        private readonly IExchangeOperationsServiceClient _exchangeOperationsServiceClient;
        private readonly IAssetsReadModelRepository _assetsService;
        private readonly ILog _log;

        public AccrualPeriodProcesor(
            string feeClientId, 
            IClientAccrualRepository clientAccrualRepository, 
            IBonusAccrualRepository bonusAccrualRepository, 
            IExchangeOperationsServiceClient exchangeOperationsServiceClient,
            IAssetsReadModelRepository assetsService,
            ILogFactory logFactory)
        {
            _feeClientId = feeClientId;
            _clientAccrualRepository = clientAccrualRepository;
            _bonusAccrualRepository = bonusAccrualRepository;
            _exchangeOperationsServiceClient = exchangeOperationsServiceClient;
            _assetsService = assetsService;
            _log = logFactory.CreateLog(this);
        }

        public async Task Process(IAccrualPeriod period)
        {
            var data = await _bonusAccrualRepository.GetData(period.ClientId, period.StartDt, period.EndDt);

            foreach (var assetGroup in data.GroupBy(x => x.AssetId))
            {
                var assetId = assetGroup.Key;

                var periodByAssetItem = await _clientAccrualRepository.GetClientAccrual(period.Id, assetId);

                // if completed then we do nothing
                if (periodByAssetItem != null && periodByAssetItem.Completed)
                {
                    continue;
                }

                // if null we creating new record for bonus transferring
                if (periodByAssetItem == null)
                {
                    periodByAssetItem = await _clientAccrualRepository.Create(period.Id, period.ClientId, assetId, assetGroup.Sum(x => x.TradeVolume), assetGroup.Sum(x => x.Bonus));
                }

                await ProcessMeTransfer(periodByAssetItem.MeId, periodByAssetItem.ClientId, periodByAssetItem.AssetId, periodByAssetItem.Bonus);

                await _clientAccrualRepository.SetCompleted(periodByAssetItem.Id);
            }
        }

        private async Task ProcessMeTransfer(string id, string clientId, string assetId, decimal amount)
        {
            var asset = _assetsService.TryGet(assetId);

            if (asset != null)
            {
                amount = amount.TruncateDecimalPlaces(asset.Accuracy);
            }
            
            var result = await _exchangeOperationsServiceClient.TransferAsync(clientId, _feeClientId, (double)amount, assetId, transactionId: id);

            if (result.IsOk())
                return;

            if (result.IsDuplicated())
            {
                _log.Warning(nameof(ProcessMeTransfer), "Me returned duplicated error", null,  $"Client: {clientId}, id: {id}");
                return;
            }

            throw new Exception($"Failed to process ME transfer, ME id: {id}, client : {clientId}, code: {result?.Code}, msg: {result?.Message}");
        }
    }
}
