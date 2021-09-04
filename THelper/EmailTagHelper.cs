using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace dotNetCoreV2.Providers.THelper
{
    public class EmailTagHelper : TagHelper
    {
        private readonly string EmailDomain = "gmail.com";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var Target = content.GetContent() + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + Target);
            output.Content.SetContent(Target);
        }
    }
}
