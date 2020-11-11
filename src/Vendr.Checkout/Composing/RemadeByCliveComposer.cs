using Umbraco.Core.Composing;
using Vendr.Checkout.Composing;
using Vendr.Core.Composing;
using Vendr.Core.Events.Notification;
using Vendr.RemadeByClive.Events.Handlers;

namespace Vendr.RemadeByClive.Composing
{
    [ComposeAfter(typeof(VendrCheckoutComposer))]
    public class RemadeByCliveComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.WithNotificationEvent<OrderSavedNotification>()
                .RegisterHandler<OrderSavedChangedHandler>();
        }
    }
}
