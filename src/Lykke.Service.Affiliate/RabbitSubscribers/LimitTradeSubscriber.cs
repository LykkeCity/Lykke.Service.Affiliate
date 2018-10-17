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
    public class LimitTradeSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;

        private readonly RabbitMeSettings _rabbitConfig;
        private RabbitMqSubscriber<LimitQueueItem> _subscriber;
        private readonly IPaidFeeQueueWriter _paidFeeQueueWriter;
        private readonly ILogFactory _logFactory;

        public LimitTradeSubscriber(
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
                _rabbitConfig.ExchangeLimit,
                _rabbitConfig.QueueLimit);

            settings.MakeDurable();

            try
            {
                _subscriber = new RabbitMqSubscriber<LimitQueueItem>(_logFactory, settings, 
                        new DeadQueueErrorHandlingStrategy(_logFactory, settings))
                    .SetMessageDeserializer(new JsonMessageDeserializer<LimitQueueItem>())
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

        private async Task ProcessMessage(LimitQueueItem tradeItem)
        {
            foreach (var order in tradeItem.Orders)
                foreach (var trade in order.Trades)
                {
                    if (trade.Fees == null) continue;

                    foreach (var item in trade.Fees)
                    {
                        if (item.Transfer != null)
                        {
                            try
                            {
                                await _paidFeeQueueWriter.AddPaidFee(Guid.NewGuid(), item.Transfer.Asset,
                                    item.Transfer.FromClientId, item.Transfer.ToClientId, (decimal)item.Transfer.Volume,
                                    item.Transfer.Date, order.Order.Id, trade.ClientId, trade.OppositeClientId, (decimal)trade.OppositeVolume);
                            }
                            catch (Exception e)
                            {
                                _log.Error(nameof(ProcessMessage), e, item.Transfer.ToJson());
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
