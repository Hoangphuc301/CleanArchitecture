using System;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities;

public partial class News
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public string? Thumbnail { get; set; }

    public string? Author { get; set; }

    public int? ViewCount { get; set; }

    public bool? IsHot { get; set; }

    public bool? Status { get; set; }

    public DateTime? PublishDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<MenuNews> MenuNews { get; set; } = new List<MenuNews>();
}
