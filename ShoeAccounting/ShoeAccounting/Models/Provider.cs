using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string ProviderTitle { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
