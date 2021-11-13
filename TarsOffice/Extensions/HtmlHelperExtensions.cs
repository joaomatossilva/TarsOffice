using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;

namespace TarsOffice.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent BeginTag<T>(this IHtmlHelper<T> htmlHelper, string tag, IDictionary<string, object> htmlAttributes = null)
        {
            var builder = new TagBuilder(tag);
            builder.MergeAttributes(htmlAttributes);
            return builder;
        }
    }
}
