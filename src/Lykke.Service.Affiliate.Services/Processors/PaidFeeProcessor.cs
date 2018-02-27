using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;
using Lykke.Service.Affiliate.Core.Services.Processors;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ClientAccount.Client.AutorestClient;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.Affiliate.Services.Processors
{
    public class PaidFeeProcessor : IPaidFeeProcessor
    {
        private const decimal OneAffiliateShare = 0.5M;
        private const decimal BothAffiliateShare = 0.25M;

        private readonly IPaidFeeRepository _paidFeeRepository;
        private readonly IBonusAccrualRepository _bonusAccrualRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IClientAccountService _clientAccountService;

        public PaidFeeProcessor(IPaidFeeRepository paidFeeRepository, IBonusAccrualRepository bonusAccrualRepository, IMemoryCache memoryCache, IClientAccountService clientAccountService)
        {
            _paidFeeRepository = paidFeeRepository;
            _bonusAccrualRepository = bonusAccrualRepository;
            _memoryCache = memoryCache;
            _clientAccountService = clientAccountService;
        }

        public async Task Process(PaidFeeQueueItem item)
        {
            var paidFeeId = item.Id.ToString();

            await _paidFeeRepository.Create(paidFeeId, item.AssetId, item.FromClient, item.ToClient, item.Volume,
                item.Order, item.TradeClient, item.TradeOppositeClient, item.TradeVolume);

            // do not process disabled assets
            if (_memoryCache.Get<IDisabledAsset>(Constants.GetCacheDisabledAssetKey(item.AssetId)) != null)
                return;

            var clientAffiliate = GetAffiliate(await GetClientId(item.TradeClient));
            var oppositeClientAffiliate = GetAffiliate(await GetClientId(item.TradeOppositeClient));

            var clientHasAffiliate = !string.IsNullOrWhiteSpace(clientAffiliate);
            var oppositeClientHasAffiliate = !string.IsNullOrWhiteSpace(oppositeClientAffiliate);

            if (clientHasAffiliate && oppositeClientHasAffiliate)
            {
                await AddBothFeeBonus(clientAffiliate, oppositeClientAffiliate, paidFeeId, item);
            }
            else if (clientHasAffiliate)
            {
                await AddOneFeeBonus(clientAffiliate, paidFeeId, item);
            }
            else if (oppositeClientHasAffiliate)
            {
                await AddOneFeeBonus(oppositeClientAffiliate, paidFeeId, item);
            }
        }

        private async Task AddOneFeeBonus(string affiliateId, string paidFeeId, PaidFeeQueueItem item)
        {
            var bonus = item.Volume * OneAffiliateShare;

            await _bonusAccrualRepository.Create(paidFeeId, affiliateId, item.AssetId, item.TradeVolume, bonus);
        }

        private async Task AddBothFeeBonus(string firstAffiliateId, string secondAffiliateId, string paidFeeId, PaidFeeQueueItem item)
        {
            var bonus = item.Volume * BothAffiliateShare;

            await Task.WhenAll(
                _bonusAccrualRepository.Create(paidFeeId, firstAffiliateId, item.AssetId, item.TradeVolume, bonus),
                _bonusAccrualRepository.Create(paidFeeId, secondAffiliateId, item.AssetId, item.TradeVolume, bonus)
            );
        }

        private string GetAffiliate(string clientId)
        {
            return _memoryCache.Get<IReferral>(Constants.GetCacheReferralKey(clientId))?.AffiliateId;
        }

        private Task<string> GetClientId(string walletId)
        {
            return _clientAccountService.GetClientByWalletAsync(walletId);
        }
    }
}
