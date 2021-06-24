using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Helpers
{
    public static class CommonHelper
    {
        public static string RemoveSpecificChar(this string str)
        {
            return !string.IsNullOrEmpty(str) ? str.Replace("'", "").Replace(";", "") : str;
        }
    }
}
