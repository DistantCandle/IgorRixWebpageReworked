using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;
using IgorRixWebpage.Web.Models.NestedElements;
using IgorRixWebpage.Web.Models.CompositionModels;
using IgorRixWebpage.Web.Models;
using IgorRixWebpage.Web;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Microsoft.AspNetCore.Http;

namespace IgorRixWebpage.Web.Helpers
{
    public class BlockMapper
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly IPublishedUrlProvider _publishedUrlProvider;
        private readonly Umbraco.Cms.Core.Web.IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContentTypeService _contentTypeService;
        private readonly IDataTypeService _dataTypeService;

        public BlockMapper(
            IPublishedValueFallback publishedValueFallback,
            IPublishedUrlProvider publishedUrlProvider,
            Umbraco.Cms.Core.Web.IUmbracoContextAccessor umbracoContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IContentTypeService contentTypeService,
            IDataTypeService dataTypeService)
        {
            _publishedValueFallback = publishedValueFallback;
            _publishedUrlProvider = publishedUrlProvider;
            _umbracoContextAccessor = umbracoContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _contentTypeService = contentTypeService;
            _dataTypeService = dataTypeService;
        }

        // Page-level mapper - uncomment when ready to wire up PageContentCompositionModel
        public PageContentCompositionModel MapPageContent(BlockListModel? blockList)
        {
            var model = new PageContentCompositionModel();
            if (blockList == null)
            {
                Console.WriteLine("Null Exception on PageContentComposition");
                return model;
            }

            model.Blocks = blockList.Select(block => block.Content.ContentType.Alias switch
            {
                "pageBanner" => (object?)MapPageBannerBlock(block),
                "sectionHeader" => (object?)MapSectionHeaderBlock(block),
                "sectionText" => (object?)MapSectionTextBlock(block),
                "accordionContainer" => (object?)MapAccordionContainerBlock(block),
                "cardComponentContainer" => (object?)MapCardComponentContainerBlock(block),
                _ => null
            })
            .Where(b => b != null)
            .ToList()!;

            return model;
        }

        public PageBanner? MapPageBannerBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                Console.WriteLine("Null Exception on MapPageBannerBlock");
                return null;
            }

            var content = block.Content;
            // var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var container = new PageBanner
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
            };

            return container;
        }

        public SectionHeader? MapSectionHeaderBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                Console.WriteLine("Null Exception on MapSectionHeaderBlock");
                return null;
            }

            var content = block.Content;
            var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var container = new SectionHeader
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
                Image = media?.MediaUrl(_publishedUrlProvider),
                ImageAltText = content.Value<string>(_publishedValueFallback, "imageAltText") ?? string.Empty,
            };

            return container;
        }

        public SectionText? MapSectionTextBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                Console.WriteLine("Null Exception on MapSectionTextBlock");
                return null;
            }

            var content = block.Content;
            var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var container = new SectionText
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
                Image = media?.MediaUrl(_publishedUrlProvider),
                ImageAltText = content.Value<string>(_publishedValueFallback, "imageAltText") ?? string.Empty,
            };

            return container;
        }

        // Accordion Container Block Method
        public AccordionContainer? MapAccordionContainerBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                return null;
            }

            var content = block.Content;
            // var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var container = new AccordionContainer
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                Items = content.Value<IEnumerable<BlockListItem>>(_publishedValueFallback, "items")?
                    .Select(MapAccordionItemBlock)
                    .Where(item => item != null)
                    .Cast<AccordionItem>()
                    .ToList()
            };
            return container;
        }

        // Accordion Item Block Method
        public AccordionItem? MapAccordionItemBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                return null;
            }

            var content = block.Content;
            // var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var item = new AccordionItem
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,

            };
            return item;
        }

        // Card Component Container Block Method
        public CardComponentContainer? MapCardComponentContainerBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                return null;
            }

            var content = block.Content;
            // var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var container = new CardComponentContainer
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
                Items = content.Value<IEnumerable<BlockListItem>>(_publishedValueFallback, "items")?
                    .Select(MapCardComponentItemBlock)
                    .Where(item => item != null)
                    .Cast<CardComponentItem>()
                    .ToList()
            };
            return container;
        }

        // Card Component Item Block Method
        public CardComponentItem? MapCardComponentItemBlock(BlockListItem block)
        {
            if (block == null || block.Content == null)
            {
                return null;
            }

            var content = block.Content;
            // var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");

            var item = new CardComponentItem
            {
                Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
                SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
                Summary = content.Value<string>(_publishedValueFallback, "summary") ?? string.Empty,
                Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
            };
            return item;
        }
    }
}


//Declarations:

// "accordionContainer" => (object?)MapAccordionContainerBlock(block),
// "headerComponent" => (object?)MapHeaderComponentBlock(block),

// public HeaderComponent? MapHeaderComponentBlock(BlockListItem block)
// {
//     if (block == null || block.Content == null)
//     {
//         return null;
//     }
//     var content = block.Content;
//     var media = content.Value<MediaWithCrops>(_publishedValueFallback, "image");
//     var container = new HeaderComponent
//     {
//         Image = media?.MediaUrl(_publishedUrlProvider),
//         ImageAltText = content.Value<string>(_publishedValueFallback, "imageAltText") ?? string.Empty,
//         Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
//         SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
//         Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
//     };
//     return container;
// }


// Image = media?.MediaUrl(_publishedUrlProvider),
// ImageAltText = content.Value<string>(_publishedValueFallback, "imageAltText") ?? string.Empty,
// Title = content.Value<string>(_publishedValueFallback, "title") ?? string.Empty,
// SubTitle = content.Value<string>(_publishedValueFallback, "subTitle") ?? string.Empty,
// Button = content.Value<IEnumerable<Link>>(_publishedValueFallback, "button")?
// Body = content.Value<string>(_publishedValueFallback, "body") ?? string.Empty,
//    .Select(link => new Link
//    {
//        Url = link.Url,
//        Name = link.Name,
//        Target = link.Target
//    })
//    .ToList(),