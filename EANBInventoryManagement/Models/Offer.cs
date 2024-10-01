using System;
using System.Collections.Generic;

namespace EANBInventoryManagement.Models;

public partial class Offer
{
    public int OfferId { get; set; }

    public int OfferUserId { get; set; }

    public int? RequestUserId { get; set; }

    public string Name { get; set; } = null!;

    public string Amount { get; set; } = null!;

    public string State { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? RequestedItemId { get; set; }

    public virtual User OfferUser { get; set; } = null!;

    public virtual User? RequestUser { get; set; }
}
