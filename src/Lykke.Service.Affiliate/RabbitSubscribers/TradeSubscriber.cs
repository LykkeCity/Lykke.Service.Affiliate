using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Azure;
using Lykke.Service.Affiliate.RabbitSubscribers.Contracts;
using Lykke.Service.Affiliate.Settings.ServiceSettings;

namespace Lykke.Service.Affiliate.RabbitSubscribers
{
    public class TradeSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;

        private readonly RabbitMeSettings _rabbitConfig;
        private RabbitMqSubscriber<TradeQueueItem> _subscriber;
        private readonly IPaidFeeQueueWriter _paidFeeQueueWriter;
        private readonly ILogFactory _logFactory;

        public TradeSubscriber(
            RabbitMeSettings config,
            ILogFactory logFactory, IPaidFeeQueueWriter paidFeeQueueWriter)
        {
            _rabbitConfig = config;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
            _paidFeeQueueWriter = paidFeeQueueWriter;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.ForSubscriber(
                _rabbitConfig.ConnectionString,
                _rabbitConfig.ExchangeTrade,
                _rabbitConfig.QueueTrade);

            settings.MakeDurable();

            try
            {
                _subscriber = new RabbitMqSubscriber<TradeQueueItem>(
                        _logFactory, settings,
                        new ResilientErrorHandlingStrategy(_logFactory, settings,
                            retryTimeout: TimeSpan.FromSeconds(20),
                            retryNum: 3,
                            next: new DeadQueueErrorHandlingStrategy(_logFactory, settings)))
                    .SetMessageDeserializer(new JsonMessageDeserializer<TradeQueueItem>())
                    .SetMessageReadStrategy(new MessageReadQueueStrategy())
                    .Subscribe(ProcessMessage)
                    .CreateDefaultBinding()
                    .Start();
            }
            catch (Exception ex)
            {

                _log.Error(nameof(Start), ex);
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
                                                       item.Transfer.Date, queueMessage.Order.Id, trade.MarketClientId, trade.LimitClientId, (decimal)trade.LimitVolume);
                            }
                            catch (Exception e)
                            {
                                _log.Error(nameof(ProcessMessage), e, item.Transfer.ToJson());
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
