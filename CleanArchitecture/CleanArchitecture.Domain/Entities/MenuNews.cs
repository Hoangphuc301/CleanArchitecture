using System;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities;

public partial class MenuNews
{
    public int MenuId { get; set; }

    public int NewsId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public string? Note { get; set; }

    public virtual Menus Menu { get; set; } = null!;

    public virtual News News { get; set; } = null!;
}
