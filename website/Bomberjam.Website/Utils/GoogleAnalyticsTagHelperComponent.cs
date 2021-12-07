using System;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Utils
{
    public class GoogleAnalyticsTagHelperComponent : TagHelperComponent
    {
        private readonly IOptions<TelemetryOptions> _telemetryOptions;

        public GoogleAnalyticsTagHelperComponent(IOptions<TelemetryOptions> telemetryOptions)
        {
            this._telemetryOptions = telemetryOptions;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase) && this._telemetryOptions.Value.GoogleAnalytics is { Length: > 0 } trackingCode)
            {
                output.PostContent
                    .AppendHtml("<script async src='https://www.googletagmanager.com/gtag/js?id=")
                    .AppendHtml(trackingCode)
                    .AppendHtml("'></script><script>window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments)}gtag('js',new Date());gtag('config','")
                    .AppendHtml(trackingCode)
                    .AppendHtml("');</script>");
            }
        }
    }
}