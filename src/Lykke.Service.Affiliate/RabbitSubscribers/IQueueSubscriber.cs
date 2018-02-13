using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Affiliate.RabbitSubscribers
{
    public interface IQueueSubscriber : IDisposable
    {
        void Start();
        void Stop();
    }
}
