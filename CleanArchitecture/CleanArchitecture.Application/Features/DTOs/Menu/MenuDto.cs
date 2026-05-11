namespace CleanArchitecture.Application.Features.DTOs.Menu
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; } = null!;
        public string? Slug { get; set; }
        public int? DisplayOrder { get; set; }
    }
}