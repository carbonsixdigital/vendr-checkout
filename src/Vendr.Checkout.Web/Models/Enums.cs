using System.ComponentModel;

namespace Vendr.Checkout.Web.Models
{
    public class Enums
    {
        public enum Media
        {
            [Description("none")]
            None,
            [Description("webp")]
            WebP
        }

        public enum CropAlias
        {
            NoCrop
        }
    }
} 