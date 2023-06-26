using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DevMS.App.Extensions
{
    public class EmailTagHelper :TagHelper
    {
        public string EmailDomain { get; set; } = "matheus.sousa";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent() + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mail:to" + target);
            output.Content.SetContent(target);            
        }
    }
}
