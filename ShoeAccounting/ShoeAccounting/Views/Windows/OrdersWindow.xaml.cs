using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using System.ComponentModel;
using System.Windows;
using static ShoeAccounting.Controllers.OrderController;

namespace ShoeAccounting.Views.Windows
{
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
            OrdersList = GetOrders();
            InitializeComponent();

            DataContext = this;
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
                    OrdersList = GetOrders();
                }
            }
        }
    }
}
