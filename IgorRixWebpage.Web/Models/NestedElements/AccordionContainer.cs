using Umbraco.Cms.Core.Models;

namespace IgorRixWebpage.Web.Models.NestedElements
{
    public class AccordionContainer
    {
        public string? Title { get; set; }
        public List<AccordionItem>? Items { get; set; }
    }

    public class AccordionItem
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}