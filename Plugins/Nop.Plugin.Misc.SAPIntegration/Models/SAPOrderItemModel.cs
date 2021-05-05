using Newtonsoft.Json;


namespace Nop.Plugin.Misc.SAPIntegration.Models
{
    public partial class SAPOrderItemModel
    {
        [JsonProperty("OrderNumber")]
        public int OrderNumber { get; set; }

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("Quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("ItemWeight")]
        public decimal ItemWeight { get; set; }
    }
}
