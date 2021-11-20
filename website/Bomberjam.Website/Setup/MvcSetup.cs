using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Setup
{
    public class MvcSetup : IConfigureOptions<RouteOptions>, IConfigureOptions<ResponseCompressionOptions>, IConfigureOptions<FormOptions>, IConfigureOptions<KestrelServerOptions>
    {
        public void Configure(RouteOptions options)
        {
            options.LowercaseUrls = true;
        }

        public void Configure(ResponseCompressionOptions options)
        {
            options.EnableForHttps = true;
        }

        public void Configure(FormOptions options)
        {
            options.MultipartBodyLengthLimit = Constants.GeneralMaxUploadSize;
        }

        public void Configure(KestrelServerOptions options)
        {
            options.Limits.MaxRequestBodySize = Constants.GeneralMaxUploadSize;
        }
    }
}