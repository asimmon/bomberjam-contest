using System;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Utils
{
    public class GoogleAnalyticsTagHelperComponent : TagHelperComponent
    {
        private readonly IOptions<GoogleAnalyticsOptions> _googleAnalyticsOptions;

        public GoogleAnalyticsTagHelperComponent(IOptions<GoogleAnalyticsOptions> googleAnalyticsOptions)
        {
            this._googleAnalyticsOptions = googleAnalyticsOptions;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                var trackingCode = this._googleAnalyticsOptions.Value.TrackingCode;
                if (!string.IsNullOrEmpty(trackingCode))
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
}