// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Affiliate.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AddDisabledAssetModel
    {
        /// <summary>
        /// Initializes a new instance of the AddDisabledAssetModel class.
        /// </summary>
        public AddDisabledAssetModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AddDisabledAssetModel class.
        /// </summary>
        public AddDisabledAssetModel(string assetId = default(string))
        {
            AssetId = assetId;
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

    }
}