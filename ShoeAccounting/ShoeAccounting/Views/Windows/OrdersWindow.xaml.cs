using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShoeAccounting.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window, INotifyPropertyChanged
    {
        private List<Order> _ordersList;
        public List<Order> OrdersList
        {
            get => _ordersList;
            set
            {
                _ordersList = value;
                OnPropertyChanged();
            }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName="")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public OrdersWindow()
        {
            CurrentUser = UserContext.CurrentUser;
            OrdersList = LoadOrders();
            InitializeComponent();

            DataContext = this;
        }

        public List<Order> LoadOrders()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Order> orders = context.Orders
                    .Include(o => o.PickUpPoint)
                    .OrderByDescending(o => o.OrderDateMake)
                    .ToList();
                return orders;
            }
        }

        private void backToCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            new CatalogWindow().Show();
            this.Close();
        }

        private void newOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new OrderManagmentWindow();
            if (editWindow.ShowDialog() == true)
            {
                if (Window.GetWindow(this) is OrdersWindow orderWindow)
                {
                    OrdersList = orderWindow.LoadOrders();
                }
            }
        }
    }
}
