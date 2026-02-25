using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoeAccounting.Controllers
{
    public class CategoryController
    {
        static public List<Category> GetCategories()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Category> categories = context.Categories.ToList();

                return categories;
            }
        }
    }
}
