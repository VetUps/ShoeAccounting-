using Microsoft.Win32;
using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для ProductManagment.xaml
    /// </summary>
    public partial class ProductManagment : Window, INotifyPropertyChanged
    {
        private bool _isProductNew;
        public bool IsProductNew
        {
            get => _isProductNew;
            set => _isProductNew = value;
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get => _categories;
            set => _categories = value;
        }

        private List<Manufacturer> _manufacturers;
        public List<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set => _manufacturers = value;
        }

        private List<Provider> _providers;
        public List<Provider> Providers
        {
            get => _providers;
            set => _providers = value;
        }

        private Product _currentProduct;

        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ProductManagment(Product product = null)
        {
            InitializeComponent();

            if (product == null)
            {
                CurrentProduct = new Product();
                IsProductNew = true;
            }
            else
            {
                CurrentProduct = product;
                IsProductNew = false;
            }

            LoadComboBoxData();
            DataContext = this;
        }

        private void LoadComboBoxData()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                Categories = context.Categories.ToList();
                Manufacturers = context.Manufacturers.ToList();
                Providers = context.Providers.ToList();
            }
        }

        private void saveProductButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{CurrentProduct.ProductTitle}");
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loadProductImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                CurrentProduct.ProductPhoto = File.ReadAllBytes(dialog.FileName);

                var binding = productImage.GetBindingExpression(Image.SourceProperty);
                binding?.UpdateTarget();

                MessageBox.Show("Изображение загружено!");
            }
        }
    }
}
