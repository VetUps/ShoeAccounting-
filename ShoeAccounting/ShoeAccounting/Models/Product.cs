using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class Product
{
    public string ProductArticle { get; set; } = null!;

    public string ProductTitle { get; set; } = null!;

    public string ProductUnit { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int? ProviderId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? CategoryId { get; set; }

    public double? ProductDiscount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public string? ProductDescription { get; set; }

    public byte[]? ProductPhoto { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Provider? Provider { get; set; }
    public decimal ProductPriceWithDiscount
    {
        get => ProductPrice * (decimal)(ProductDiscount / 100);
    }
}
