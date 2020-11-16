using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Vendr.Checkout.Web.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveReturns(this string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                s = s.Replace("\r", "");
                s = s.Replace("\n", " ");
            }
            return s;
        }

        public static string RemoveReturnsAndWhiteSpace(this string s)
        {
            string s1 = s.RemoveReturns();
            if (!string.IsNullOrWhiteSpace(s1))
                s1 = Regex.Replace(s1, @"\s+", "");

            return s1;
        }
    }
}