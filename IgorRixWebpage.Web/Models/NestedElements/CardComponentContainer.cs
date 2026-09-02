using Umbraco.Cms.Core.Models;

namespace IgorRixWebpage.Web.Models.NestedElements
{
    public class CardComponentContainer
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Body { get; set; }
        public List<CardComponentItem>? Items { get; set; }
    }

    public class CardComponentItem
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
    }
}