using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedModels;
using Vendr.Checkout.Web.Models;
using Vendr.Core;
using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Web.Extensions
{
    public static class PublishedContentExtensions
    {
        public static IPublishedContent FindImage(this IPublishedContent node)
        {
            if (node == null) return null;
            if (node.HasValue("listingImage")) return node.Value<IPublishedContent>("listingImage");
            if (node.HasValue("image")) return node.Value<IPublishedContent>("image");
            if (node.HasValue("heroImage")) return node.Value<IPublishedContent>("heroImage");
            if (node.HasValue("photos")) return node.Value<IPublishedContent>("photos");
            return null;
        }

        public static string GetCropUrlWithFormat(
          this IPublishedContent node,
          Enums.CropAlias cropAlias,
          Enums.Media format = Enums.Media.None,
          bool cacheBuster = false,
          int? quality = 75,
          bool useCropDimensions = true,
          ImageCropMode? imageCropMode = null,
          ImageCropAnchor? imageCropAnchor = null)
        {
            if (node == null) return null;

            if (node.Url().EndsWith(".svg"))
                return node.Url();


            if (cropAlias == Enums.CropAlias.NoCrop)
                return node.GetCropUrl(
                    furtherOptions: "&format=" + format,
                    cacheBuster: cacheBuster,
                    quality: quality);

            switch (format)
            {
                case Enums.Media.WebP:
                    var webP = node.GetCropUrl(
                        cropAlias: cropAlias.GetDescription(),
                        furtherOptions: "&format=" + format,
                        cacheBuster: cacheBuster,
                        useCropDimensions: useCropDimensions,
                        quality: quality,
                        imageCropMode: imageCropMode,
                        imageCropAnchor: imageCropAnchor);

                    if (webP != null) return webP;
                    break;
                default:
                    var crop = node.GetCropUrl(
                        cropAlias: cropAlias.GetDescription(),
                        cacheBuster: cacheBuster,
                        useCropDimensions: useCropDimensions,
                        quality: quality,
                        imageCropMode: imageCropMode,
                        imageCropAnchor: imageCropAnchor);

                    if (crop != null) return crop;
                    break;
            }

            return node.Url();
        }

        public static string GetCropUrlWithFormat(
            this IPublishedContent node,
            Enums.Media format = Enums.Media.None,
            bool cacheBuster = false,
            int? quality = 75,
            bool useCropDimensions = false,
            ImageCropMode? imageCropMode = null,
            ImageCropAnchor? imageCropAnchor = null)
        {
            if (node == null) return null;

            if (node.Url().EndsWith(".svg"))
            {
                var crop = node.GetCropUrl(
                    cacheBuster: cacheBuster,
                    quality: quality);

                if (crop != null) return crop;
                return node.Url();
            }

            switch (format)
            {
                case Enums.Media.WebP:
                    var webP = node.GetCropUrl(
                        furtherOptions: "&format=" + format,
                        cacheBuster: cacheBuster,
                        quality: quality,
                        useCropDimensions: useCropDimensions,
                        imageCropMode: imageCropMode,
                        imageCropAnchor: imageCropAnchor);

                    if (webP != null) return webP;
                    break;
                default:
                    var crop = node.GetCropUrl(
                        cacheBuster: cacheBuster,
                        quality: quality,
                        imageCropMode: imageCropMode,
                        useCropDimensions: useCropDimensions,
                        imageCropAnchor: imageCropAnchor);

                    if (crop != null) return crop;
                    break;
            }

            return node.Url();
        }

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

        public static Product GetProduct(this IPublishedContent content, Guid productReference)
        {
            var umbracoContextFactory = DependencyResolver.Current.GetService<IUmbracoContextFactory>();
            if (umbracoContextFactory == null) return null;
            using (var cref = umbracoContextFactory.EnsureUmbracoContext())
                return cref.UmbracoContext?.Content?.GetById(productReference) as Product;
        }
        public static string GetCurrency(this IPublishedContent content)
        {
            var currencyId = content.GetStore()?.BaseCurrencyId;
            if (currencyId != null)
                return VendrApi.Instance?.GetCurrency(currencyId.Value)?.Code;

            return string.Empty;
        }
        public static string GetAltText(this IPublishedContent content, string altTextAlias = "altText")
        => content != null && content.HasValue(altTextAlias) ? content.Value<string>(altTextAlias) : string.Empty;

    }
}
