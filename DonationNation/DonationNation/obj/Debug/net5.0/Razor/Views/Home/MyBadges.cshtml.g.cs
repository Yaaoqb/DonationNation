#pragma checksum "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e24f250e047e7cb902cd0653ecd1afc7d8082244"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(DonationNation.Pages.Home.Views_Home_MyBadges), @"mvc.1.0.view", @"/Views/Home/MyBadges.cshtml")]
namespace DonationNation.Pages.Home
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using DonationNation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using DonationNation.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using DonationNation.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using DonationNation.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\_ViewImports.cshtml"
using DonationNation.Extensions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e24f250e047e7cb902cd0653ecd1afc7d8082244", @"/Views/Home/MyBadges.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"61a46ae973e9fde17597c5ce9f5b7ca726363f34", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_MyBadges : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Badge>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
  
    ViewData["Title"] = "MyBadges";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n<div class=\"container mt-3 justify-content-center\">\r\n    <h1>My Badges</h1>\r\n");
#nullable restore
#line 12 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
     if (Model != null & Model.Count() > 0)
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
         foreach (var batch in Model.Batch(3))
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"row justify-content-center\">\r\n");
#nullable restore
#line 17 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
                 foreach (var item in batch)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"card m-3 DonationNation-event\" style=\"width: 18rem;\">\r\n                        <img class=\"card-img-top\"");
            BeginWriteAttribute("src", " src=\"", 524, "\"", 541, 1);
#nullable restore
#line 20 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
WriteAttributeValue("", 530, item.Image, 530, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" width=\"70\" height=\"200\" alt=\"DonationNation event picture\">\r\n                        <div class=\"card-body text-center\">\r\n                            <h5 class=\"card-title\">");
#nullable restore
#line 22 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
                                              Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 25 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n");
#nullable restore
#line 27 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "C:\Users\Yaaqob\source\repos\DonationNation\DonationNation\Views\Home\MyBadges.cshtml"
         
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Badge>> Html { get; private set; }
    }
}
#pragma warning restore 1591
