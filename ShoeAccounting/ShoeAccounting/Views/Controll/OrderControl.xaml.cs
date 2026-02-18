using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using ShoeAccounting.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeAccounting.Views.Controll
{
    /// <summary>
    /// Логика взаимодействия для OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public OrderControl()
        {
            InitializeComponent();
            CurrentUser = UserContext.CurrentUser;

            ContextMenuManagment();
        }

        private void orderPositionsButton_Click(object sender, RoutedEventArgs e)
        {
            Order currnetOrder = DataContext as Order;
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<OrderPosition> orderPositions = context.OrderPositions.Where(op => op.OrderId == currnetOrder.OrderId).ToList();

                string orderPositionsString = "Товары в заказе:\n";
                foreach (OrderPosition orderPosition in orderPositions)
                    orderPositionsString += $"{orderPosition.ProductArticle} - {orderPosition.ProductQuantity} шт.\n";

                MessageBox.Show(orderPositionsString, "Состав заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ContextMenuManagment()
        {
            if (CurrentUser.UserRole == "Администратор")
                orderManagmentContextMenu.IsEnabled = true;
            else
                orderManagmentContextMenu.IsEnabled = false;
        }

        private void redactOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new OrderManagmentWindow(DataContext as Order);
            if (editWindow.ShowDialog() == true)
            {
                if (Window.GetWindow(this) is OrdersWindow orderWindow)
                {
                    orderWindow.OrdersList = orderWindow.LoadOrders();
                }
            }
        }

        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ShoesDbContext())
            {
                Order currentOrder = DataContext as Order;

                if (MessageBox.Show("Вы точно хотите удалить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var orderToDelete = context.Orders
                        .Include(o => o.OrderPositions)
                        .FirstOrDefault(o => o.OrderId == currentOrder.OrderId);

                    if (orderToDelete.OrderPositions.Any())
                    {
                        context.OrderPositions.RemoveRange(orderToDelete.OrderPositions);
                    }

                    context.Orders.Remove(orderToDelete);
                    context.SaveChanges();

                    if (Window.GetWindow(this) is OrdersWindow orderWindow)
                    {
                        orderWindow.OrdersList = orderWindow.LoadOrders();
                    }
                }

              
            }
        }
    }
}
