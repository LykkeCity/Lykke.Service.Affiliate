// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Affiliate.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StatisticItemModel
    {
        /// <summary>
        /// Initializes a new instance of the StatisticItemModel class.
        /// </summary>
        public StatisticItemModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StatisticItemModel class.
        /// </summary>
        public StatisticItemModel(decimal tradeVolume, decimal bonusVolume, string assetId = default(string))
        {
            AssetId = assetId;
            TradeVolume = tradeVolume;
            BonusVolume = bonusVolume;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TradeVolume")]
        public decimal TradeVolume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BonusVolume")]
        public decimal BonusVolume { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
