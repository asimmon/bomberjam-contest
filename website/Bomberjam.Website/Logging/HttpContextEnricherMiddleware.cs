using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Bomberjam.Website.Logging
{
    public class HttpContextEnricherMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpContextEnrichersFactory _enrichersFactory;

        public HttpContextEnricherMiddleware(RequestDelegate next, HttpContextEnrichersFactory contextEnrichersFactory)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
            this._enrichersFactory = contextEnrichersFactory ?? throw new ArgumentNullException(nameof(contextEnrichersFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            var enrichers = this._enrichersFactory(context).ToArray();

            if (enrichers.Length == 0)
            {
                await this._next(context);
            }
            else
            {
                using (LogContext.Push(enrichers))
                {
                    await this._next(context);
                }
            }
        }
    }
}