using ShoeAccounting.Models;
using ShoeAccounting.Utils;
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
        private Product _currentProduct;
        public Product CurrentProduct
        {
            get => _currentProduct;
            set => _currentProduct = value;
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public ProductControl()
        {
            InitializeComponent();

            CurrentProduct = DataContext as Product;
            CurrentUser = UserContext.CurrentUser;
        }

        private void redactProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
