using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Vendr.Core;
using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Web.Extensions
{
    public static class PublishedContentExtensions
    {     
        public static Home GetHomepage(this IPublishedContent content)
            => content.AncestorOrSelf<Home>();      
        public static VendrCheckoutCheckoutPage GetCheckoutTypedPage(this IPublishedContent content)
            => content.GetHomepage()?.FirstChild<VendrCheckoutCheckoutPage>(x => x.IsPublished());

        public static CartPage GetCartPage(this IPublishedContent content)
            => content.GetHomepage()?.FirstChild<CartPage>(x => x.IsPublished() && x.TemplateId > 0);

        public static OrderReadOnly GetCurrentOrder(this IPublishedContent content)
            => content.GetStore() != null ? VendrApi.Instance?.GetCurrentOrder(content.GetStore().Id) : null;

        public static string GetProductReference(this IPublishedContent content)
            => content.Key.ToString();

        public static IProductSnapshot AsVendrProduct(this IPublishedContent content)
            => content.GetStore() != null ? VendrApi.Instance?.GetProduct(content.GetProductReference()) : null;

        public static Price CalculatePrice(this IPublishedContent content)
            => content.GetStore() != null ? content?.AsVendrProduct()?.CalculatePrice() : null;   
    }
}
