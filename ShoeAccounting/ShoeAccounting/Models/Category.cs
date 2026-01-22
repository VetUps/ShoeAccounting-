using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryTitle { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
