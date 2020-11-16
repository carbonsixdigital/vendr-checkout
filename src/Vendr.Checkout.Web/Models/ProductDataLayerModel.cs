using System;
using Newtonsoft.Json;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Vendr.Checkout.Web.Extensions;

namespace Vendr.Checkout.Web.Models
{
    [Serializable]
    public class ProductDataLayerModel
    {
        public ProductDataLayerModel(Product args, int position = 0, string list = null)
        {
            if (args == null) return;

            Name = args.Name;
            Id = args.Sku;

            var store = args.GetStore();
            if (store != null)
                Price = args.CalculatePrice().WithTax;

            Brand = args.AncestorOrSelf<Home>()?.FindTitle();
            Category = args.Parent.FindTitle();
            Position = position + 1;
            List = list;
        }

        [JsonProperty(propertyName: "name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "price")]
        public decimal Price { get; set; }

        [JsonProperty(propertyName: "brand")]
        public string Brand { get; set; }

        [JsonProperty(propertyName: "category")]
        public string Category { get; set; }

        [JsonProperty(propertyName: "variant")]
        public string Variant { get; set; }

        [JsonProperty(propertyName: "position")]
        public int Position { get; set; }

        [JsonProperty(propertyName: "list")]
        public string List { get; set; }
    }
}