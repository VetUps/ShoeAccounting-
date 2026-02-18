using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    public virtual ICollection<OrderPosition> OrderPositions { get; set; } = new List<OrderPosition>();

    public virtual Provider? Provider { get; set; }
    [NotMapped]
    public decimal ProductPriceWithDiscount
    {
        get => ProductPrice - (ProductPrice * (decimal)(ProductDiscount / 100));
        set;
    }
}
