using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.RabbitSubscribers.Contracts;
using Lykke.Service.Affiliate.Settings;
using Lykke.Service.Affiliate.Settings.ServiceSettings;

namespace Lykke.Service.Affiliate.RabbitSubscribers
{
    public class TradeSubscriber : IQueueSubscriber
    {
#if DEBUG
        private const bool QueueDurable = false;
#else
        private const bool QueueDurable = true;
#endif

        private readonly ILog _log;

        private readonly RabbitMeSettings _rabbitConfig;
        private RabbitMqSubscriber<TradeQueueItem> _subscriber;
        private readonly IPaidFeeQueueWriter _paidFeeQueueWriter;

        public TradeSubscriber(
            RabbitMeSettings config,
            ILog log, IPaidFeeQueueWriter paidFeeQueueWriter)
        {
            _rabbitConfig = config;
            _log = log;
            _paidFeeQueueWriter = paidFeeQueueWriter;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForSubscriber(
                _rabbitConfig.ConnectionString,
                _rabbitConfig.ExchangeTrade,
                _rabbitConfig.QueueTrade);

            if (QueueDurable)
                settings.MakeDurable();

            try
            {
                _subscriber = new RabbitMqSubscriber<TradeQueueItem>(
                        settings,
                        new ResilientErrorHandlingStrategy(_log, settings,
                            retryTimeout: TimeSpan.FromSeconds(20),
                            retryNum: 3,
                            next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                    .SetMessageDeserializer(new JsonMessageDeserializer<TradeQueueItem>())
                    .SetMessageReadStrategy(new MessageReadQueueStrategy())
                    .Subscribe(ProcessMessage)
                    .CreateDefaultBinding()
                    .SetLogger(_log)
                    .Start();
            }
            catch (Exception ex)
            {

                _log.WriteErrorAsync(nameof(TradeSubscriber), nameof(Start), null, ex).Wait();
                throw;
            }
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        private async Task ProcessMessage(TradeQueueItem queueMessage)
        {
            foreach (var trade in queueMessage.Trades)
            {
                if (trade.Fees != null)
                {
                    foreach (var item in trade.Fees)
                    {
                        if (item.Transfer != null)
                        {
                            try
                            {
                                await _paidFeeQueueWriter.AddPaidFee(Guid.NewGuid(), item.Transfer.Asset,
                                                       item.Transfer.FromClientId, item.Transfer.ToClientId, (decimal)item.Transfer.Volume,
                                                       item.Transfer.Date, queueMessage.Order.Id);
                            }
                            catch (Exception e)
                            {
                                await _log.WriteErrorAsync(nameof(TradeSubscriber), nameof(ProcessMessage), item.Transfer.ToJson(), e);
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
