using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str); //this function does cool global reduce things!
    }
}
