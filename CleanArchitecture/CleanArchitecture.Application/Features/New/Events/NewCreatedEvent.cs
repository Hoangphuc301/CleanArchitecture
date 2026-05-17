
namespace CleanArchitecture.Application.Features.New.Events
{
    public class NewCreatedEvent
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = null!;
        public string? Slug { get; set; } 
        public string Content { get; set; } = null!;
    }
}
