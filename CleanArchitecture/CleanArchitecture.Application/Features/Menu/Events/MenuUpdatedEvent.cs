namespace CleanArchitecture.Application.Features.Menu.Events
{
    public class MenuUpdatedEvent
    {
        public int MenuId { get; set; }
        public string NewMenuName { get; set; } = null!;
        public string? Slug { get; set; }
        public int? DisplayOrder { get; set; }
    }
}