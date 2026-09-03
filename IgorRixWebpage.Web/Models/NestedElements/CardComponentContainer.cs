using Umbraco.Cms.Core.Models;
using IgorRixWebpage.Web.DTO;

namespace IgorRixWebpage.Web.Models.NestedElements
{
    public class CardComponentContainer
    {
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Body { get; set; }
        public List<ServiceCardDto> ServiceCards { get; set; } = new();
        public List<ServicePageDto> ServicePages { get; set; } = new();
    }
}