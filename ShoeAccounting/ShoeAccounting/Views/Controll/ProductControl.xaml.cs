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
    /// Логика взаимодействия для ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public ProductControl()
        {
            InitializeComponent();
            CurrentUser = UserContext.CurrentUser;

            ContextMenuManagment();
        }

        private void ContextMenuManagment()
        {
            if (CurrentUser.UserRole == "Администратор")
                productManagmentContextMenu.IsEnabled = true;
            else 
                productManagmentContextMenu.IsEnabled = false;
        }

        private void redactProductButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ProductManagment(DataContext as Product);
            if (editWindow.ShowDialog() == true)
            {
                if (Window.GetWindow(this) is CatalogWindow catalogWindow)
                {
                    catalogWindow.LoadProducts();
                }
            }
        }

        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
