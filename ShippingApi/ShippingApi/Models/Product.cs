using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ShippingApi.Models
{
    public class Product
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("inventoryQuantity")]
        public int InventoryQuantity { get; set; }

        [JsonProperty("shipOnWeekends")]
        public bool ShipOnWeekends { get; set; }

        [JsonProperty("maxBusinessDaysToShip")]
        public int MaxBusinessDaysToShip { get; set; }

        [JsonProperty("shipDate")]
        public DateTime ShipDate { get; set; }

        [JsonPropertyAttribute("shipDateFormatted")]
        public string ShipDateFormatted { get; set; }

        public string Type => "product";
    }
}
