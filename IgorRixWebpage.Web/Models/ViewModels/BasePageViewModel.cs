using IgorRixWebpage.Web.Models.CompositionModels;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace IgorRixWebpage.Web.Models.ViewModels
{
    public class BasePageViewModel
    {
        // Property must be public and uncommented so views and mappers can access it
        public PageContentCompositionModel? PageContentCompositionModel { get; set; }
        public IPublishedContent? CurrentPage { get; set; }
    }
}
