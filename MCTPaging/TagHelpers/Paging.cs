using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using MCTPaging.Model;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MCTPaging.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "paging-model")]
    public class Pageing : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public Pageing(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingData PagingModel { get; set; }     
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public string PageClassLabel { get; set; }
        public string PageClassLinks { get; set; }


        public override void Process(TagHelperContext helperContext, TagHelperOutput helperOutput)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder outerDiv = new TagBuilder("div");

            int startPage;
            int endPage;
            if (PagingModel.TotalPages > 1)
            {
                if (PagingModel.TotalPages <= PagingModel.LinksPerPage)
                {
                    startPage = 1;
                    endPage = PagingModel.TotalPages;
                }
                else
                {
                    if (PagingModel.CurrentPage + PagingModel.LinksPerPage - 1 > PagingModel.TotalPages)
                    {
                        startPage = PagingModel.CurrentPage - ((PagingModel.CurrentPage + PagingModel.LinksPerPage - 1) 
                            - PagingModel.TotalPages);
                        endPage = (PagingModel.CurrentPage + PagingModel.LinksPerPage - 1) - 
                            ((PagingModel.CurrentPage + PagingModel.LinksPerPage - 1) - PagingModel.TotalPages);
                    }
                    else
                    {                      
                        if (PagingModel.LinksPerPage !=2)
                        {
                            startPage = PagingModel.CurrentPage - (PagingModel.LinksPerPage / 2);
                            if (startPage < 1)
                            {
                                startPage = 1;
                            }
                            endPage = startPage + PagingModel.LinksPerPage - 1;
                        }                    
                        else
                        {
                            startPage = PagingModel.CurrentPage;
                            endPage = PagingModel.CurrentPage + PagingModel.LinksPerPage - 1;
                        }

                    }

                }
                TagBuilder labelDiv;
                labelDiv = new TagBuilder("div");
                labelDiv.AddCssClass(PageClassLabel);
                labelDiv.InnerHtml.Append($"Showing {PagingModel.CurrentPage} of { PagingModel.TotalPages}");
                outerDiv.InnerHtml.AppendHtml(labelDiv);
                TagBuilder linkDiv = new TagBuilder("div");
                linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("First", 1));
                for (int i = startPage; i <= endPage; i++)
                {
                    linkDiv.InnerHtml.AppendHtml(GeneratePageLinks(i.ToString(), i));
                }

                linkDiv.InnerHtml.AppendHtml(GeneratePageLinks("Last", PagingModel.TotalPages));
                linkDiv.AddCssClass(PageClassLinks);
                outerDiv.InnerHtml.AppendHtml(linkDiv);
                helperOutput.Content.AppendHtml(outerDiv.InnerHtml);
            }
        }

        private TagBuilder GeneratePageLinks(string iterator, int pageNumber)
        {
            string url;
            TagBuilder tag;
            tag = new TagBuilder("a");
            url = PagingModel.UrlParams.Replace("-", pageNumber.ToString());
            tag.Attributes["href"] = url;
            tag.AddCssClass(PageClass);
            if (iterator != "First" && iterator != "Last")
            {
                tag.AddCssClass(Convert.ToInt32(iterator) == PagingModel.CurrentPage ? PageClassSelected : PageClassNormal);
            }
            else
            {
                tag.AddCssClass(pageNumber == PagingModel.CurrentPage ? PageClassSelected : PageClassNormal);
            }
            tag.InnerHtml.Append(iterator.ToString());
            return tag;

        }


    }
}
