namespace CleanArchitecture.Application.Features.Menu.Events
{
    public class MenuCreatedEvent
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; } = null!;
        public string? Slug { get; set; } 
        public int? DisplayOrder { get; set; }
    }
}