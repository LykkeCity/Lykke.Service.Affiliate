// Code generated by Microsoft (R) AutoRest Code Generator 1.1.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.Affiliate.Client.AutorestClient.Models
{
    using Lykke.Service;
    using Lykke.Service.Affiliate;
    using Lykke.Service.Affiliate.Client;
    using Lykke.Service.Affiliate.Client.AutorestClient;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RegisterLinkModel
    {
        /// <summary>
        /// Initializes a new instance of the RegisterLinkModel class.
        /// </summary>
        public RegisterLinkModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RegisterLinkModel class.
        /// </summary>
        public RegisterLinkModel(string partnerId = default(string), string redirectUrl = default(string))
        {
            PartnerId = partnerId;
            RedirectUrl = redirectUrl;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PartnerId")]
        public string PartnerId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RedirectUrl")]
        public string RedirectUrl { get; set; }

    }
}