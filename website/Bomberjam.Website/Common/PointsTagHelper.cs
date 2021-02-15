using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bomberjam.Website.Common
{
    [HtmlTargetElement("span", Attributes = PointsAttributeName)]
    public class PointsTagHelper : TagHelper
    {
        private const string PointsAttributeName = "format-points";

        [HtmlAttributeName(PointsAttributeName)]
        public float Points { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = (this.Points / 30f).ToString("0.000");
            output.Content.SetContent(content);
        }
    }
}