﻿using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using MessagePack;
using Newtonsoft.Json;

namespace Lykke.Service.Affiliate.RabbitSubscribers.Contracts
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class LimitQueueItem
    {
        [JsonProperty("orders")] public List<LimitOrderWithTrades> Orders { get; set; }

        [MessagePackObject(keyAsPropertyName: true)]
        public class LimitOrderWithTrades
        {
            [JsonProperty("order")] public LimitOrder Order { get; set; }

            [JsonProperty("trades")] public List<LimitTradeInfo> Trades { get; set; }
        }

        [MessagePackObject(keyAsPropertyName: true)]
        public class LimitOrder
        {
            [JsonProperty("externalId")] public string Id { get; set; }

            [JsonProperty("id")] public string MatchingId { get; set; }

            [JsonProperty("assetPairId")] public string AssetPairId { get; set; }

            [JsonProperty("clientId")] public string ClientId { get; set; }

            [JsonProperty("volume")] public double Volume { get; set; }

            [JsonProperty("price")] public double Price { get; set; }

            [JsonProperty("status")] public string Status { get; set; }

            [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }

            [JsonProperty("registered")] public DateTime Registered { get; set; }

            [JsonProperty("remainingVolume")] public double RemainingVolume { get; set; }

            public bool Straight { get; set; } = true;
        }

        [MessagePackObject(keyAsPropertyName: true)]
        public class LimitTradeInfo
        {
            [JsonProperty("clientId")] public string ClientId { get; set; }

            [JsonProperty("asset")] public string Asset { get; set; }

            [JsonProperty("volume")] public double Volume { get; set; }

            [JsonProperty("price")] public double Price { get; set; }

            [JsonProperty("timestamp")] public DateTime Timestamp { get; set; }

            [JsonProperty("oppositeOrderId")] public string OppositeOrderId { get; set; }

            [JsonProperty("oppositeOrderExternalId")]
            public string OppositeOrderExternalId { get; set; }

            [JsonProperty("oppositeAsset")] public string OppositeAsset { get; set; }

            [JsonProperty("oppositeClientId")] public string OppositeClientId { get; set; }

            [JsonProperty("oppositeVolume")] public double OppositeVolume { get; set; }

            [CanBeNull]
            [JsonProperty("fees")]
            public List<Fee> Fees { get; set; }
        }
    }
}
