using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        private List<string> _providersFiltrationList; 
        public List<string> ProvidersFiltrationList
        {
            get => _providersFiltrationList;
            set => _providersFiltrationList = value;
        }

        private User? _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public List<Product> _productsList;
        public List<Product> ProductsList
        {
            get => _productsList;
            set
            {
                _productsList = value; 
                if (value != null)
                {
                    shoesItemControl.ItemsSource = ProductsList;
                }
            }
        }

        private string _currentSortMethod;
        public string CurrentSortMethod
        {
            get => _currentSortMethod;
            set => _currentSortMethod = value;
        }

        private string _currentFilterMethod;
        public string CurrentFilterMethod
        {
            get => _currentFilterMethod;
            set => _currentFilterMethod = value;
        }

        private string _currentSearchText;
        public string CurrentSearchText
        {
            get => _currentSearchText;
            set => _currentSearchText = value;
        }

        public CatalogWindow()
        {
            CurrentUser = UserContext.CurrentUser;
            ProvidersFiltrationList = LoadProvidersFiltrationList();

            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Window_Loaded;
            SelectDefaultComboBoxValues();

            ProductsList = LoadProducts();
        }

        public List<Product> LoadProducts()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                IQueryable<Product> products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Provider)
                    .Include(p => p.Manufacturer);

                if (CurrentFilterMethod != "Все поставщики")
                    products = products.Where(p => p.Provider.ProviderTitle == CurrentFilterMethod);

                if (CurrentSortMethod == "По возрастанию (кол-во на складе)")
                    products = products.OrderBy(p => p.ProductQuantityInStock);

                else if (CurrentSortMethod == "По убыванию (кол-во на складе)")
                    products = products.OrderByDescending(p => p.ProductQuantityInStock);

                if (!string.IsNullOrEmpty(CurrentSearchText))
                    products = products.Where(
                        p => p.ProductTitle.ToLower().Trim().Contains(CurrentSearchText) ||
                        p.ProductDescription.ToLower().Trim().Contains(CurrentSearchText) ||
                        p.Category.CategoryTitle.ToLower().Trim().Contains(CurrentSearchText)
                    );

                return products.ToList();
            }
        }

        private List<Provider> LoadProviders()
        {
            using (ShoesDbContext context = new ShoesDbContext ())
            {
                List<Provider> providers = context.Providers.ToList();
                return providers;
            }
        }

        private List<string> LoadProvidersFiltrationList()
        {
            List<Provider> providersList = LoadProviders();
            List<string> providersFiltrationList = new List<string>() { "Все поставщики" };

            foreach (Provider provider in providersList)
                providersFiltrationList.Add(provider.ProviderTitle);

            return providersFiltrationList;
        }

        private void backToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void SelectDefaultComboBoxValues()
        {
            if (sortComboBox != null && sortComboBox.Items.Count > 0)
                sortComboBox.SelectedIndex = 0;

            if (filterComboBox != null && filterComboBox.Items.Count > 0)
                filterComboBox.SelectedItem = "Все поставщики";
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSortMethod = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            ProductsList = LoadProducts();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFilterMethod = (sender as ComboBox).SelectedItem as string;
            ProductsList = LoadProducts();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrentSearchText = (sender as TextBox).Text.ToLower().Trim();
            ProductsList = LoadProducts();
        }
    }
}
