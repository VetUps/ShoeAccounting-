using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeAccounting.Models;

public partial class PickUpPoint
{
    public int PickUpPointId { get; set; }

    public string PickUpPointPostalCode { get; set; } = null!;

    public string PickUpPointCity { get; set; } = null!;

    public string PickUpPointStreet { get; set; } = null!;

    public string? PickUpPointHome { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string DisplayAddress
    {
        get
        {
            var parts = new List<string>
            {
                PickUpPointPostalCode?.Trim(),
                PickUpPointCity?.Trim(),
                PickUpPointStreet?.Trim(),
                PickUpPointHome?.Trim()
            }
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

            return string.Join(", ", parts);
        }

        set;
    }
}
