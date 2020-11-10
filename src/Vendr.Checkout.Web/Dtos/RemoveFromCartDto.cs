using System;

namespace Vendr.Checkout.Web.Dtos
{
    public class RemoveFromCartDto
    {
        public Guid OrderLineId { get; set; }
    }
}
