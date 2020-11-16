using Newtonsoft.Json;
using Vendr.Checkout.Web.Models;

namespace Umbraco.Web.PublishedModels
{
    public partial class Product
	{
		public string DataLayerModelJson(int position = 0, string list = null) => JsonConvert.SerializeObject(new ProductDataLayerModel(this, position, list));
	}
}