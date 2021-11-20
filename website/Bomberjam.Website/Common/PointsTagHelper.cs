using System;
using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bomberjam.Website.Common
{
    [HtmlTargetElement("span", Attributes = FormatPointsAttributeName)]
    [HtmlTargetElement("span", Attributes = ShowSignAttributeName)]
    public class PointsTagHelper : TagHelper
    {
        private const string FormatPointsAttributeName = "format-points";
        private const string ShowSignAttributeName = "show-sign";

        [HtmlAttributeName(FormatPointsAttributeName)]
        public float FormatPoints { get; set; }

        [HtmlAttributeName(ShowSignAttributeName)]
        public bool ShowSign { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ShowSign)
            {
                var absPoints = Math.Abs(this.FormatPoints);
                var sign = this.FormatPoints >= 0 ? "+" : "-";
                var content = sign + (absPoints / 30f).ToString("0.000", CultureInfo.InvariantCulture);
                output.Content.SetContent(content);
            }
            else
            {
                var content = (this.FormatPoints / 30f).ToString("0.000", CultureInfo.InvariantCulture);
                output.Content.SetContent(content);
            }
        }
    }
}