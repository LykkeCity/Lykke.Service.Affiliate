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
    public class LimitTradeSubscriber : IQueueSubscriber
    {
        private readonly ILog _log;

        private readonly RabbitMeSettings _rabbitConfig;
        private RabbitMqSubscriber<LimitQueueItem> _subscriber;
        private readonly IPaidFeeQueueWriter _paidFeeQueueWriter;

        public LimitTradeSubscriber(
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
                _rabbitConfig.ExchangeLimit,
                _rabbitConfig.QueueLimit);

            settings.MakeDurable();

            try
            {
                _subscriber = new RabbitMqSubscriber<LimitQueueItem>(settings, new DeadQueueErrorHandlingStrategy(_log, settings))
                    .SetMessageDeserializer(new JsonMessageDeserializer<LimitQueueItem>())
                    .SetMessageReadStrategy(new MessageReadQueueStrategy())
                    .Subscribe(ProcessMessage)
                    .CreateDefaultBinding()
                    .SetLogger(_log)
                    .Start();
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(LimitTradeSubscriber), nameof(Start), null, ex).Wait();
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
                                    item.Transfer.Date, order.Order.Id, trade.ClientId, trade.OppositeClientId);
                            }
                            catch (Exception e)
                            {
                                await _log.WriteErrorAsync(nameof(TradeSubscriber), nameof(ProcessMessage), item.Transfer.ToJson(), e);
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
