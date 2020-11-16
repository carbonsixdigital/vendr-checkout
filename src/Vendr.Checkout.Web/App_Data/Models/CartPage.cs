using Umbraco.Core.Models.PublishedContent;
using Vendr.Checkout.Web.Extensions;
using Vendr.Core.Models;

namespace Umbraco.Web.PublishedModels
{
    public partial class CartPage
	{
		public VendrCheckoutCheckoutPage CheckoutPage => this.GetCheckoutTypedPage();

		public OrderReadOnly Order => this.GetCurrentOrder();

	}
}
