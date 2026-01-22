using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoeAccounting.Utils
{
    public static class UserContext
    {
        public static User? CurrentUser { get; set; } = null;
    }
}
