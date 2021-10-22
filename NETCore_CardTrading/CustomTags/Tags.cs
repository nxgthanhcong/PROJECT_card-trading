using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore_CardTrading.CustomTags
{
    [HtmlTargetElement(Attributes = "disabled-link")]
    public class DisabledLinkTagHelper : TagHelper
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("onclick", "event.preventDefault()");
            return base.ProcessAsync(context, output);
        }
    }
}
