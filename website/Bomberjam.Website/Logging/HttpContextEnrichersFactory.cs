using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Serilog.Core;

namespace Bomberjam.Website.Logging
{
    public delegate IEnumerable<ILogEventEnricher> HttpContextEnrichersFactory(HttpContext httpContext);
}