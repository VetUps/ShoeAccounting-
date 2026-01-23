using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeAccounting.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserRole { get; set; }

    public string UserFirstname { get; set; } = null!;

    public string UserLastname { get; set; } = null!;

    public string? UserPatronymic { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string UserFullName => $"{UserLastname} {UserFirstname} {UserPatronymic}";
}
