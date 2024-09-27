using System;
using System.Collections.Generic;

namespace EANBInventoryManagement.Models;

public partial class RequestedItem
{
    public int RequestedItemId { get; set; }

    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public int Amount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Event Event { get; set; } = null!;
}
