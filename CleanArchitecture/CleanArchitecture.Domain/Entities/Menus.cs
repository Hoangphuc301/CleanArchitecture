using System;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities;

public partial class Menus
{
    public int MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<MenuNews> MenuNews { get; set; } = new List<MenuNews>();
}
