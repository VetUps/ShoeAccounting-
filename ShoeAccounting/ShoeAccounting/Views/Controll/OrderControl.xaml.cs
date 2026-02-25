using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using ShoeAccounting.Views.Windows;
using System.Windows;
using System.Windows.Controls;
using static ShoeAccounting.Controllers.OrderController;

namespace ShoeAccounting.Views.Controll
{
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
            List<OrderPosition> orderPositions = GetOrderPositionsByOrder(currnetOrder);

            string orderPositionsString = "Товары в заказе:\n";
            foreach (OrderPosition orderPosition in orderPositions)
                orderPositionsString += $"{orderPosition.ProductArticle} - {orderPosition.ProductQuantity} шт.\n";

            MessageBox.Show(orderPositionsString, "Состав заказа", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    orderWindow.OrdersList = GetOrders();
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
                    DeleteOrder(currentOrder);
                }
            }
        }
    }
}
