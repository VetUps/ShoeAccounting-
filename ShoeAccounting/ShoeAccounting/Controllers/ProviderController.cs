using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoeAccounting.Controllers
{
    public class ProviderController
    {
        static public List<Provider> GetProviders()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Provider> providers = context.Providers.ToList();
                return providers;
            }
        }
    }
}
