using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShoeAccounting.Utils
{
    public class TextForegroundDiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double discount = (double)value;

            if (discount > 15)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextForegroundDiscountStockConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double discount = (double)values[0];
            int stock = (int)values[1];

            if (stock == 0)
                return Brushes.LightBlue;

            if (discount > 15)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
            return new SolidColorBrush(Colors.Black);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DiscountVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double discount = (double)value;
            bool result = discount > 0;

            if (Invert)
                result = !result;

            return result ? Visibility.Visible:Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BlobToImageConverter : IValueConverter
    {
        private static readonly BitmapImage _defaultImage;

        static BlobToImageConverter()
        {
            _defaultImage = new BitmapImage();
            _defaultImage.BeginInit();
            _defaultImage.UriSource = new Uri("pack://application:,,,/Resources/Images/picture.png");
            _defaultImage.CacheOption = BitmapCacheOption.OnLoad;
            _defaultImage.EndInit();
            _defaultImage.Freeze();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] imageData && imageData.Length > 0)
            {
                try
                {
                    using (var stream = new MemoryStream(imageData))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                        image.Freeze();
                        return image;
                    }
                }
                catch
                {
                    return _defaultImage;
                }
            }

            return _defaultImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CatalogFiltersVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userRole = (string)value;

            if (userRole == "Менеджер" || userRole == "Администратор")
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UserRoleToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string userRole = (string)value;

            if (userRole == "Администратор")
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ManagmentSettingsToTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNew = (bool)value;

            return isNew ? "Добавление товара" : "Редактирование товара";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
