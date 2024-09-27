using System;
using System.Collections.Generic;

namespace EANBInventoryManagement.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int UserId { get; set; }

    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<RequestedItem> RequestedItems { get; set; } = new List<RequestedItem>();

    public virtual User User { get; set; } = null!;
}
