using System;
using System.Web.Mvc;
using System.Web.WebPages;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;
using Vendr.Checkout.Web.Dtos;
using Vendr.Checkout.Web.Extensions;
using Vendr.Core;
using Vendr.Core.Exceptions;
using Vendr.Core.Services;
using Vendr.Core.Session;

namespace Vendr.Checkout.Web.Controllers
{
    public class CartSurfaceController : SurfaceController
    {
        private readonly ISessionManager _sessionManager;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWorkProvider _uowProvider;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public CartSurfaceController(ISessionManager sessionManager, IOrderService orderService, IUnitOfWorkProvider uowProvider, IUmbracoContextFactory umbracoContextFactory)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _uowProvider = uowProvider ?? throw new ArgumentNullException(nameof(uowProvider));
            _umbracoContextFactory = umbracoContextFactory ?? throw new ArgumentNullException(nameof(umbracoContextFactory));
        }

        [ChildActionOnly]
        public ActionResult CartCount()
        {
            var store = CurrentPage.GetStore();
            if (store == null) return new EmptyResult();

            var order = _sessionManager.GetCurrentOrder(store.Id);
            return PartialView("~/Views/Partials/Chrome/CartCount.cshtml", order?.TotalQuantity ?? 0);
        }

        [HttpPost]
        public ActionResult AddToCart(AddToCartDto postModel)
        {
            try
            {
                using (var uow = _uowProvider.Create())
                {
                    var store = CurrentPage.GetStore();
                    var order = _sessionManager.GetOrCreateCurrentOrder(store.Id)
                        .AsWritable(uow);

                  //  var product = GetProduct(postModel.ProductReference.As<Guid>());
                  //  if (product != null)
                   // {
                       // if (product.IsBundle)
                      //      AddBundle(postModel, order, product);
                       // else
                      //      order.AddProduct(postModel.ProductReference, 1);
                   // }
                   // else
                        order.AddProduct(postModel.ProductReference, 1);

                    _orderService.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("productReference", "Failed to add product to cart");
                Logger.Error(typeof(CartSurfaceController), ex, "AddToCart Error");
                return CurrentUmbracoPage();
            }

            var cartPage = CurrentPage.GetCartPage();
            if (cartPage != null)
                return Redirect(cartPage.Url());

            TempData["addedProductReference"] = postModel.ProductReference;
            return RedirectToCurrentUmbracoPage();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCart(UpdateCartDto postModel)
        {
            try
            {
                using (var uow = _uowProvider.Create())
                {
                    var store = CurrentPage.GetStore();
                    var order = _sessionManager.GetOrCreateCurrentOrder(store.Id)
                        .AsWritable(uow);

                    foreach (var orderLine in postModel.OrderLines)
                    {
                        order.WithOrderLine(orderLine.Id)
                            .SetQuantity(orderLine.Quantity);
                    }

                    _orderService.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                switch (ex.Message)
                {
                    case "IsProductAvailableForPurchase":
                        ModelState.AddModelError("IsProductAvailableForPurchase", Umbraco.GetDictionaryValue("Checkout.IsProductAvailableForPurchaseError"));
                        break;
                }

                return CurrentUmbracoPage();
            }

            TempData["cartUpdated"] = "true";

            return RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        public ActionResult RemoveFromCart(RemoveFromCartDto postModel)
        {
            try
            {
                using (var uow = _uowProvider.Create())
                {
                    var store = CurrentPage.GetStore();
                    var order = _sessionManager.GetOrCreateCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .RemoveOrderLine(postModel.OrderLineId);

                    _orderService.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException)
            {
                ModelState.AddModelError("productReference", "Failed to remove cart item");

                return CurrentUmbracoPage();
            }

            return RedirectToCurrentUmbracoPage();
        }

        //private void AddBundle(AddToCartDto postModel, Vendr.Core.Models.Order order, Product product)
        //{
        //    var bundle = order.GetBundle(product.Key.ToString());
        //    if (bundle != null)
        //    {
        //        order.WithOrderLine(bundle.Id)
        //            .IncrementQuantity(1);
        //    }
        //    else
        //    {
        //        order.AddProduct(postModel.ProductReference, 1, product.Key.ToString());
        //        foreach (var bundledProduct in product.BundledProducts)
        //            order.AddProductToBundle(product.Key.ToString(), bundledProduct.GetProductReference(), 1);
        //    }
        //}

        //private Product GetProduct(Guid productReference)
        //{
        //    using (var cref = _umbracoContextFactory.EnsureUmbracoContext())
        //        return cref.UmbracoContext?.Content?.GetById(productReference) as Product;
        //}
    }
}
