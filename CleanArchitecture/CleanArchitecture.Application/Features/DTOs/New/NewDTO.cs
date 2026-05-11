
namespace CleanArchitecture.Application.Features.DTOs.New
{
    public class NewDTO
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }

    }
}
