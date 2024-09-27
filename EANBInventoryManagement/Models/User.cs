using System;
using System.Collections.Generic;

namespace EANBInventoryManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Offer> OfferOfferUsers { get; set; } = new List<Offer>();

    public virtual ICollection<Offer> OfferRequestUsers { get; set; } = new List<Offer>();
}
