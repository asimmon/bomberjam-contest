using System;
using Microsoft.AspNetCore.Builder;

namespace Bomberjam.Website.Logging
{
    public static class HttpContextEnricherMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpContextEnricher(this IApplicationBuilder builder, HttpContextEnrichersFactory contextEnrichersFactory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (contextEnrichersFactory == null)
            {
                throw new ArgumentNullException(nameof(contextEnrichersFactory));
            }

            return builder.UseMiddleware<HttpContextEnricherMiddleware>(contextEnrichersFactory);
        }
    }
}