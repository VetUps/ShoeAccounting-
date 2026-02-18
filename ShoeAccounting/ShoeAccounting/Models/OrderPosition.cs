using System;
using System.Collections.Generic;

namespace ShoeAccounting.Models;

public partial class OrderPosition
{
    public int OrderPositionId { get; set; }

    public int OrderId { get; set; }

    public string ProductArticle { get; set; } = null!;

    public int ProductQuantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductArticleNavigation { get; set; } = null!;
}
