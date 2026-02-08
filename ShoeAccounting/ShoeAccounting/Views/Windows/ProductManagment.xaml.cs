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
    /// Логика взаимодействия для ProductManagment.xaml
    /// </summary>
    public partial class ProductManagment : Window
    {
        private bool _isProductNew;
        public bool IsProductNew
        {
            get => _isProductNew;
            set => _isProductNew = value;
        } 

        public ProductManagment(bool isProductNew = false)
        {
            IsProductNew = isProductNew;

            InitializeComponent();
        }
    }
}
