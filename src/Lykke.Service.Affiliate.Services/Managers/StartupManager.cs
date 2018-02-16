﻿using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Affiliate.Core;
using Lykke.Service.Affiliate.Core.Services;
using Lykke.Service.Affiliate.Core.Services.Managers;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.Affiliate.Services.Managers
{
    // NOTE: Sometimes, startup process which is expressed explicitly is not just better, 
    // but the only way. If this is your case, use this class to manage startup.
    // For example, sometimes some state should be restored before any periodical handler will be started, 
    // or any incoming message will be processed and so on.
    // Do not forget to remove As<IStartable>() and AutoActivate() from DI registartions of services, 
    // which you want to startup explicitly.

    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;
        private readonly IMemoryCache _memoryCache;
        private readonly IReferralService _referralService;

        public StartupManager(ILog log, IMemoryCache memoryCache, IReferralService referralService)
        {
            _log = log;
            _memoryCache = memoryCache;
            _referralService = referralService;
        }

        public async Task StartAsync()
        {
            var referrals = await _referralService.GetAllReferrals();

            foreach (var item in referrals)
            {
                _memoryCache.Set(Constants.GetCacheReferralKey(item.ReferralId), item);
            }

            await Task.CompletedTask;
        }
    }
}