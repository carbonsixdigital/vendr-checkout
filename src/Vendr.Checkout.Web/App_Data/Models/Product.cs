using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Vendr.Checkout.Web.Models;

namespace Umbraco.Web.PublishedModels
{
    public partial class Product
	{
		public string DataLayerModelJson(int position = 0, string list = null) => JsonConvert.SerializeObject(new ProductDataLayerModel(this, position, list));
		public IEnumerable<Product> BundledProducts => this.BundledItems?.Any() ?? false ? this.BundledItems.OfType<Product>().Where(x => x.IsPublished()) : null;
		public bool IsBundle => this.BundledProducts?.Any() ?? false;
	}
}