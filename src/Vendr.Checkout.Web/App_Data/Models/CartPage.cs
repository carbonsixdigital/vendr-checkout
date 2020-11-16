using Umbraco.Core.Models.PublishedContent;
using Vendr.Checkout.Web;
using Vendr.Checkout.Web.Extensions;
using Vendr.Core.Models;

namespace Umbraco.Web.PublishedModels
{
    public partial class CartPage
	{
		public IPublishedContent CheckoutPage => this.GetCheckoutPage();

		public OrderReadOnly Order => this.GetCurrentOrder();

	}
}
