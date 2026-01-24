using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace ShoeAccounting.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public User? CurrentUser { get; set; }
        public List<Product> Products { get; set; }

        public CatalogWindow()
        {
            CurrentUser = UserContext.CurrentUser;
            Products = LoadProducts();

            InitializeComponent();
            DataContext = this;
        }

        private List<Product> LoadProducts()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                List<Product> products = context.Products.Include(p => p.Category)
                    .Include(p => p.Provider)
                    .Include(p => p.Manufacturer)
                    .ToList();
                return products;
            }
        }

        private void backToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
