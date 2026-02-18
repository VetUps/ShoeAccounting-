using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? ProductArticle { get; set; }

    public int? ProductQuantity { get; set; }

    public DateOnly? OrderDateMake { get; set; }

    public DateOnly? OrderDateReceipt { get; set; }

    public int PickUpPointId { get; set; }

    public int UserId { get; set; }

    public string? OrderReceiptCode { get; set; }

    public string? OrderStatus { get; set; }

    public virtual PickUpPoint PickUpPoint { get; set; } = null!;

    public virtual Product? ProductArticleNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
