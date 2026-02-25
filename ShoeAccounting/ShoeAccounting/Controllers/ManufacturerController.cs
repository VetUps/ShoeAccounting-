using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoeAccounting.Controllers
{
    public class ManufacturerController
    {
        static public List<Manufacturer> GetManufacturers()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Manufacturer> manufacturers = context.Manufacturers.ToList();

                return manufacturers;
            }
        }
    }
}
