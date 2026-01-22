using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class PickUpPoint
{
    public int PickUpPointId { get; set; }

    public string PickUpPointPostalCode { get; set; } = null!;

    public string PickUpPointCity { get; set; } = null!;

    public string PickUpPointStreet { get; set; } = null!;

    public string? PickUpPointHome { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
