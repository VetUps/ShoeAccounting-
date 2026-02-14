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
using SixLabors.ImageSharp;

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
                CurrentProduct.ProductArticle = GenerateArticle();
                CurrentProduct.ProductUnit = "шт.";
            }
            else
            {
                CurrentProduct = product;
                IsProductNew = false;
                productArticleTextBox.IsEnabled = false;
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

        private string GenerateArticle()
        {
            Random random = new Random();
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char[] chars = new char[6];
            for (int i = 0; i < 6; i++)
            {
                chars[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(chars);
        }

        private void saveProductButton_Click(object sender, RoutedEventArgs e)
        {
            var bindingErrors = new List<string>();

            if (Validation.GetErrors(productPriceTextBox).Count > 0)
                bindingErrors.Add("• Некорректное значение цены");
            if (Validation.GetErrors(productDiscountTextBox).Count > 0)
                bindingErrors.Add("• Некорректное значение скидки");
            if (Validation.GetErrors(productQuantityInStockTextBox).Count > 0)
                bindingErrors.Add("• Некорректное значение количества на складе");

            if (bindingErrors.Count > 0)
            {
                string errorMessage = "Исправьте ошибки в полях ввода:\n\n" + string.Join("\n", bindingErrors);
                MessageBox.Show(errorMessage, "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> errors = new List<string>();

            // Обязательные поля

            // Название товара
            if (string.IsNullOrWhiteSpace(CurrentProduct.ProductTitle))
                errors.Add("• Наименование товара не может быть пустым");

            // Артикул
            if (string.IsNullOrWhiteSpace(CurrentProduct.ProductArticle))
            {
                errors.Add("• Артикул не может быть пустым");
            }
            else if (CurrentProduct.ProductArticle.Length != 6)
            {
                errors.Add($"• Артикул должен содержать ровно 6 символов (сейчас: {CurrentProduct.ProductArticle.Length})");
            }

            // Единица измерения
            if (string.IsNullOrWhiteSpace(CurrentProduct.ProductUnit))
                errors.Add("• Единица измерения не может быть пустой");

            // Цена
            if (CurrentProduct.ProductPrice <= 0)
                errors.Add("• Цена товара должна быть больше 0");
            else if (CurrentProduct.ProductPrice > 10000000)
                errors.Add("• Цена товара не может превышать 10 000 000 руб.");

            // Количество на складе
            if (CurrentProduct.ProductQuantityInStock < 0)
                errors.Add("• Количество на складе не может быть отрицательным");

            // Поставщик
            if (CurrentProduct.ProviderId == 0 || CurrentProduct.ProviderId == null)
                errors.Add("• Выберите поставщика");

            // Производитель
            if (CurrentProduct.ManufacturerId == 0 || CurrentProduct.ManufacturerId == null)
                errors.Add("• Выберите производителя");

            // Категория
            if (CurrentProduct.CategoryId == 0 || CurrentProduct.CategoryId == null)
                errors.Add("• Выберите категорию");

            // Скидка
            if (CurrentProduct.ProductDiscount < 0)
                errors.Add("• Скидка не может быть отрицательной");
            else if (CurrentProduct.ProductDiscount > 90)
                errors.Add("• Скидка не может превышать 90%");

            // Уникальность артикула
            if (IsProductNew)
            {
                using (ShoesDbContext context = new ShoesDbContext())
                {
                    bool articleExists = context.Products
                        .Any(p => p.ProductArticle == CurrentProduct.ProductArticle);

                    if (articleExists)
                    {
                        errors.Add($"• Товар с артикулом '{CurrentProduct.ProductArticle}' уже существует в базе данных");
                    }
                }
            }

            if (errors.Count > 0)
            {
                string errorMessage = "Исправьте следующие ошибки:\n\n" + string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (ShoesDbContext context = new ShoesDbContext())
                {
                    if (IsProductNew)
                    {
                        context.Products.Add(CurrentProduct);
                    }
                    else
                    {
                        var existingProduct = context.Products
                            .FirstOrDefault(p => p.ProductArticle == CurrentProduct.ProductArticle);

                        if (existingProduct != null)
                        {
                            context.Entry(existingProduct).CurrentValues.SetValues(CurrentProduct);
                        }
                    }

                    context.SaveChanges();
                }
                MessageBox.Show("Товар успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                using (var image = SixLabors.ImageSharp.Image.Load(dialog.FileName))
                {
                    if (image.Width == 300 && image.Height == 200)
                    {
                        CurrentProduct.ProductPhoto = File.ReadAllBytes(dialog.FileName);

                        var binding = productImage.GetBindingExpression(System.Windows.Controls.Image.SourceProperty);
                        binding?.UpdateTarget();

                        MessageBox.Show("Изображение загружено!");
                    }
                    else
                    {
                        MessageBox.Show(
                            "Изображение должно быть размером ровно 300x200 пикселей.",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            }
        }
    }
}
