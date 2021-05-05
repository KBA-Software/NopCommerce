using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nop.Plugin.Misc.SAPIntegration.Models
{
    public partial class SAPOrderModel
    {
        public SAPOrderModel()
        {
            OrderItem = new List<SAPOrderItemModel>();
        }

        [JsonProperty("OrderNumber")]
        public int OrderNumber { get; set; }

        [JsonProperty("CustFName")]
        public string CustFName { get; set; }

        [JsonProperty("CustLName")]
        public string CustLName { get; set; }

        [JsonProperty("ShipAddress")]
        public string ShipAddress { get; set; }

        [JsonProperty("ShipCity")]
        public string ShipCity { get; set; }

        [JsonProperty("ShippingStatusId")]
        public int ShippingStatusId { get; set; }

        [JsonProperty("OrderStatusId")]
        public int OrderStatusId { get; set; }

        [JsonProperty("PaymentMetod")]
        public string PaymentMetod { get; set; }

        [JsonProperty("OrderSubtotalInclTax")]
        public decimal OrderSubtotalInclTax { get; set; }

        [JsonProperty("OrderSubtotalExclTax")]
        public decimal OrderSubtotalExclTax { get; set; }

        [JsonProperty("OrderTotal")]
        public decimal OrderTotal { get; set; }

        [JsonProperty("ShippingMethod")]
        public string ShippingMethod { get; set; }

        [JsonProperty("OrderItem")]
        public List<SAPOrderItemModel> OrderItem { get; set; }
    }
}
