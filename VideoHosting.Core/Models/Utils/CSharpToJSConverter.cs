using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Utils
{
    public static class CSharpToJSConverter
    {
        public static IHtmlContent IntArrayToJSString(this HtmlHelper helper, int[] arr)
        {
            string result = "[";
            foreach (var num in arr)
            {
                if (result.Length > 1)
                    result += ",";
                result += num;
            }
            result += "]";
            return helper.Raw(result);
        }

        public static IHtmlContent StringArrayToJSString(this HtmlHelper helper, string[] strs)
        {
            string result = "[";
            foreach (var str in strs)
            {
                if (result.Length > 1)
                    result += ",";
                result += $"\'{str}\'";
            }
            result += "]";
            return helper.Raw(result);
        }

        public static IHtmlContent StringToJSString(this HtmlHelper helper, string str)
        {
            return helper.Raw(str);
        }

        public static IHtmlContent ColorArrayToJSString(this HtmlHelper helper, Color[] colors)
        {
            string result = "[";
            foreach (var color in colors)
            {
                if (result.Length > 1)
                    result += ",";
                result += string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A);
            }
            result += "]";
            return helper.Raw(result);
        }

        public static IHtmlContent ColorToJSString(this HtmlHelper helper, Color color)
        {
            return helper.Raw(string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, color.A));
        }
    }
}