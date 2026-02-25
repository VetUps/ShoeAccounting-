using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using ShoeAccounting.Views.Windows;
using System.Windows;

namespace ShoeAccounting.Controllers
{
    public class OrderController
    {
        static public OrderPosition? GetOrderPositionByProduct(Product product)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                OrderPosition? orderPosition = context.OrderPositions.FirstOrDefault(o => o.ProductArticle == product.ProductArticle);
                return orderPosition;
            }
        }

        static public List<OrderPosition> GetOrderPositionsByOrder(Order order)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<OrderPosition> orderPositions = context.OrderPositions.Where(op => op.OrderId == order.OrderId).ToList();

                return orderPositions;
            }
        }

        static public List<Order> GetOrders()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Order> orders = context.Orders
                    .Include(o => o.PickUpPoint)
                    .Include(o => o.OrderPositions)
                    .OrderByDescending(o => o.OrderDateMake)
                    .ToList();

                return orders;
            }
        }

        static public void DeleteOrder(Order order)
        {
            using (var context = new ShoesDbContext())
            {

                var orderToDelete = context.Orders
                        .Include(o => o.OrderPositions)
                        .FirstOrDefault(o => o.OrderId == order.OrderId);

                if (orderToDelete.OrderPositions.Any())
                {
                    context.OrderPositions.RemoveRange(orderToDelete.OrderPositions);
                }

                context.Orders.Remove(orderToDelete);
                context.SaveChanges();
            }
        }

        static public void CreateOrder(Order order, List<OrderPosition> orderPositions, int userId)
        {
            using (var context = new ShoesDbContext())
            {
                order.UserId = userId;
                context.Orders.Add(order);
                context.SaveChanges();

                foreach (var pos in orderPositions)
                {
                    context.OrderPositions.Add(new OrderPosition
                    {
                        OrderId = order.OrderId,
                        ProductArticle = pos.ProductArticle,
                        ProductQuantity = pos.ProductQuantity
                    });
                }
                
                context.SaveChanges();
            }
        }

        static public void UpdateOrder(Order order, List<OrderPosition> orderPositions)
        {
            using (var context = new ShoesDbContext())
            {
                var dbOrder = context.Orders
                            .Include(o => o.OrderPositions)
                            .First(o => o.OrderId == order.OrderId);

                dbOrder.OrderDateMake = order.OrderDateMake;
                dbOrder.OrderDateReceipt = order.OrderDateReceipt;
                dbOrder.PickUpPointId = order.PickUpPointId;
                dbOrder.OrderStatus = order.OrderStatus;

                context.OrderPositions.RemoveRange(dbOrder.OrderPositions);
                dbOrder.OrderPositions.Clear();

                foreach (var pos in orderPositions)
                {
                    dbOrder.OrderPositions.Add(new OrderPosition
                    {
                        ProductArticle = pos.ProductArticle,
                        ProductQuantity = pos.ProductQuantity
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
