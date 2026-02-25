using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;

namespace ShoeAccounting.Controllers
{
    public class ProductController
    {
        static public List<Product> LoadProducts(string filterMethod, string sortMethod, string searchText)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                IQueryable<Product> products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Provider)
                    .Include(p => p.Manufacturer);

                if (filterMethod != "Все поставщики")
                    products = products.Where(p => p.Provider.ProviderTitle == filterMethod);

                if (sortMethod == "По возрастанию (кол-во на складе)")
                    products = products.OrderBy(p => p.ProductQuantityInStock);

                else if (sortMethod == "По убыванию (кол-во на складе)")
                    products = products.OrderByDescending(p => p.ProductQuantityInStock);

                if (!string.IsNullOrEmpty(searchText))
                    products = products.Where(
                        p => p.ProductTitle.ToLower().Trim().Contains(searchText) ||
                        p.ProductDescription.ToLower().Trim().Contains(searchText) ||
                        p.Category.CategoryTitle.ToLower().Trim().Contains(searchText)
                    );

                return products.ToList();
            }
        }

        static public void DeleteProduct(Product product)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }

        static public void CreateProduct(Product product)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        static public void UpdateProduct(Product product)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                var existingProduct = context.Products
                            .FirstOrDefault(p => p.ProductArticle == product.ProductArticle);

                if (existingProduct != null)
                {
                    context.Entry(existingProduct).CurrentValues.SetValues(product);
                }

                context.SaveChanges();
            }
        }
    }
}
