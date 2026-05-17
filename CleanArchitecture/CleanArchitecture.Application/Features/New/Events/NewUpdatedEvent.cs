
namespace CleanArchitecture.Application.Features.New.Events
{
    public class NewUpdatedEvent
    {
        public int NewId { get; set; }
        public string NewTitle { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Content { get; set; }
    }
}
