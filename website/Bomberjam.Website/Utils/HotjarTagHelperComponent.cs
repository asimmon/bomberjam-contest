using System;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Utils
{
    public class HotjarTagHelperComponent : TagHelperComponent
    {
        private const string ScriptTagPart1 = @"<script>
    (function(h,o,t,j,a,r){
        h.hj=h.hj||function(){(h.hj.q=h.hj.q||[]).push(arguments)};
        h._hjSettings={hjid:";

        private const string ScriptTagPart2 = @",hjsv:6};
        a=o.getElementsByTagName('head')[0];
        r=o.createElement('script');r.async=1;
        r.src=t+h._hjSettings.hjid+j+h._hjSettings.hjsv;
        a.appendChild(r);
    })(window,document,'https://static.hotjar.com/c/hotjar-','.js?sv=');
</script>";

        private readonly IOptions<TelemetryOptions> _telemetryOptions;

        public HotjarTagHelperComponent(IOptions<TelemetryOptions> telemetryOptions)
        {
            this._telemetryOptions = telemetryOptions;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase) && this._telemetryOptions.Value.Hotjar is { Length: > 0 } trackingCode)
            {
                output.PostContent.AppendHtml(ScriptTagPart1).AppendHtml(trackingCode).AppendHtml(ScriptTagPart2);
            }
        }
    }
}