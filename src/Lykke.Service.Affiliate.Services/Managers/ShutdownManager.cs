using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.JobTriggers.Triggers;
using Lykke.Sdk;

namespace Lykke.Service.Affiliate.Services.Managers
{
    // NOTE: Sometimes, shutdown process should be expressed explicitly. 
    // If this is your case, use this class to manage shutdown.
    // For example, sometimes some state should be saved only after all incoming message processing and 
    // all periodical handler was stopped, and so on.
    
    public class ShutdownManager : IShutdownManager
    {
        private readonly IEnumerable<IStopable> _stopables;
        private readonly TriggerHost _triggerHost;
        private readonly ILog _log;

        public ShutdownManager(
            ILogFactory logFactory,
            IEnumerable<IStopable> stopables,
            TriggerHost triggerHost)
        {
            _stopables = stopables;
            _triggerHost = triggerHost;
            _log = logFactory.CreateLog(this);
        }

        public async Task StopAsync()
        {
            foreach (var stopable in _stopables)
                stopable.Stop();
            
            _triggerHost.Cancel();

            await Task.CompletedTask;
        }
    }
}
