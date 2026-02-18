using ShoeAccounting.Models;
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
        public OrderControl()
        {
            InitializeComponent();
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
    }
}
