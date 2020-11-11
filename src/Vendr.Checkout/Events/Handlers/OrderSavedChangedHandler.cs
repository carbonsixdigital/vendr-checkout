using System.Linq;
using Vendr.Checkout;
using Vendr.Core.Api;
using Vendr.Core.Events.Notification;

namespace Vendr.RemadeByClive.Events.Handlers
{
    public class OrderSavedChangedHandler : NotificationEventHandlerBase<OrderSavedNotification>
    {
        public override void Handle(OrderSavedNotification evt)
        {
            var order = VendrApi.Instance.GetOrder(evt.Order.Id);

            if (!order.IsFinalized)
            {
                var paymentMethods = VendrApi.Instance.GetPaymentMethodsAllowedIn(order.PaymentInfo.CountryId.Value);
                var zeroValuePaymentMethod = paymentMethods.FirstOrDefault(x => x.Alias == VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue);
                if (zeroValuePaymentMethod != null)
                {
                    if (order.TotalPrice.Value.WithTax > 0 && order.PaymentInfo.PaymentMethodId == zeroValuePaymentMethod.Id)
                    {
                        using (var uow = VendrApi.Instance.Uow.Create())
                        {
                            var writableOrder = order.AsWritable(uow)
                                .ClearPaymentMethod();

                            VendrApi.Instance.SaveOrder(writableOrder);

                            uow.Complete();
                        }
                    }
                    else if (order.TotalPrice.Value.WithTax == 0 && order.PaymentInfo.PaymentMethodId != zeroValuePaymentMethod.Id)
                    {
                        using (var uow = VendrApi.Instance.Uow.Create())
                        {
                            var writableOrder = order.AsWritable(uow)
                                .SetPaymentMethod(zeroValuePaymentMethod);

                            VendrApi.Instance.SaveOrder(writableOrder);

                            uow.Complete();
                        }
                    }
                }
            }
        }
    }
}
