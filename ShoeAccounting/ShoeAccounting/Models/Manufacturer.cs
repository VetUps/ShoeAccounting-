using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerTitle { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
