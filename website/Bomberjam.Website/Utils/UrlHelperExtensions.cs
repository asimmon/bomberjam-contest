using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bomberjam.Website.Utils
{
    public static class UrlHelperExtensions
    {
        public static string ContentAbsolute(this IUrlHelper urlHelper, HttpContext context, string contentPath)
        {
            var contentRelativeUrl = urlHelper.Content(contentPath);

            if (Uri.TryCreate(context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase, UriKind.Absolute, out var baseUrl))
            {
                return new Uri(baseUrl, contentRelativeUrl).ToString();
            }

            return contentRelativeUrl;
        }
    }
}