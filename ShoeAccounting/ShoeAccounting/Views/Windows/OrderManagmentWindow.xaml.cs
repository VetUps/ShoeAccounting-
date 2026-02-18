using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для OrderManagmentWindow.xaml
    /// </summary>
    public partial class OrderManagmentWindow : Window, INotifyPropertyChanged
    {
        private bool _isOrderNew;
        public bool IsOrderNew
        {
            get => _isOrderNew;
            set => _isOrderNew = value;
        }

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set => _products = value;
        }

        private List<PickUpPoint> _pickUpPoints;
        public List<PickUpPoint> PickUpPoints
        {
            get => _pickUpPoints;
            set => _pickUpPoints = value;
        }

        private List<string> _statuses;
        public List<string> Statuses
        {
            get => _statuses;
            set => _statuses = value;
        }

        private Order _currentOrder;
        public Order CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                OnPropertyChanged();
            }
        }

        private class OrderPositionItem : INotifyPropertyChanged
        {
            private Product _selectedProduct;
            private int _quantity = 1;

            public Product SelectedProduct
            {
                get => _selectedProduct;
                set
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ProductArticle));
                    OnPropertyChanged(nameof(ProductTitle));
                }
            }

            public string ProductArticle
            {
                get => SelectedProduct?.ProductArticle ?? string.Empty;
            }

            public string ProductTitle
            {
                get => SelectedProduct?.ProductTitle ?? string.Empty;
            }

            public int Quantity
            {
                get => _quantity;
                set
                {
                    if (value > 0)
                    {
                        _quantity = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string propertyName = "")
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly List<OrderPositionItem> _positions = new();

        public DateTime? OrderDateMakeDateTime
        {
            get => CurrentOrder.OrderDateMake.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentOrder.OrderDateMake = value.HasValue
                ? DateOnly.FromDateTime(value.Value.Date)
                : DateOnly.FromDateTime(DateTime.Today.Date);
                OnPropertyChanged();
            }
        }

        public DateTime OrderDateReceiptDateTime
        {
            get => CurrentOrder.OrderDateReceipt.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentOrder.OrderDateReceipt = DateOnly.FromDateTime(value.Date);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName="")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public OrderManagmentWindow(Order? order = null)
        {
            InitializeComponent();
            LoadComboBoxData();

            if (order == null)
            {
                CurrentOrder = new Order();
                CurrentOrder.OrderDateMake = DateOnly.FromDateTime(DateTime.Today.Date);
                IsOrderNew = true;
            }
            else
            {
                CurrentOrder = order;
                IsOrderNew = false;
                LoadExistingPositions(order);
            }

            SetOrderDatePickerLimits();
            SetDeliveryDatePickerLimits();

            DataContext = this;
            UpdatePositionsList();
        }

        private void LoadExistingPositions(Order order)
        {
            foreach (var pos in order.OrderPositions)
            {
                var product = Products.FirstOrDefault(p => p.ProductArticle == pos.ProductArticle);
                if (product != null)
                {
                    _positions.Add(new OrderPositionItem
                    {
                        Quantity = pos.ProductQuantity,
                        SelectedProduct = product,
                    });
                }
            }
        }

        private void UpdatePositionsList()
        {
            positionsListView.ItemsSource = null;
            positionsListView.ItemsSource = _positions;
        }

        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            _positions.Add(new OrderPositionItem { Quantity = 1 });
            UpdatePositionsList();
        }

        private void RemovePositionButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button btn && btn.Tag is OrderPositionItem item)
            {
                _positions.Remove(item);
                UpdatePositionsList();
            }
        }

        private void NumberValidationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, e.Text.Length - 1);
        }

        private void NumberValidationTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!text.All(char.IsDigit))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void LoadComboBoxData()
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                Products = context.Products.ToList();
                PickUpPoints = context.PickUpPoints.ToList();
                Statuses = new List<string>
                {
                    "Новый",
                    "Завершён"
                };
            }
        }

        private void SetOrderDatePickerLimits()
        {
            orderDatePicker.DisplayDateStart = DateTime.Today.Date;
            orderDatePicker.DisplayDateEnd = DateTime.Today.AddMonths(1).Date;
        }

        private void SetDeliveryDatePickerLimits()
        {
            // Базовая дата = дата заказа или сегодня, если не задана
            DateTime baseDate = OrderDateMakeDateTime?.Date ?? DateTime.Today.Date;

            // Сбрасываем дату доставки, если она выходит за границы
            if (OrderDateReceiptDateTime.Date < baseDate ||
                OrderDateReceiptDateTime.Date > baseDate.AddMonths(1).Date)
            {
                OrderDateReceiptDateTime = baseDate;
            }

            deliveryDatePicker.DisplayDateStart = baseDate;
            deliveryDatePicker.DisplayDateEnd = baseDate.AddMonths(1).Date;
        }

        private void saveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = new List<string>();

            // Обязательные поля

            // Статус заказа
            if (string.IsNullOrWhiteSpace(CurrentOrder.OrderStatus))
            {
                errors.Add("• Выберите статус заказа");
            }
            else if (CurrentOrder.OrderStatus != "Новый" && CurrentOrder.OrderStatus != "Завершён")
            {
                errors.Add("• Недопустимый статус заказа (допустимые: \"Новый\", \"Завершён\")");
            }

            // Пункт выдачи
            if (CurrentOrder.PickUpPointId <= 0)
            {
                errors.Add("• Выберите пункт выдачи");
            }
            else if (!PickUpPoints.Any(p => p.PickUpPointId == CurrentOrder.PickUpPointId))
            {
                errors.Add("• Выбранный пункт выдачи не существует в базе данных");
            }

            // Дата получения
            if (CurrentOrder.OrderDateReceipt.ToDateTime(TimeOnly.MinValue) < DateTime.Today.Date)
            { 
                errors.Add("• Дата доставки не может быть в прошлом");
            }
            else
            {
                DateTime baseDate = CurrentOrder.OrderDateMake.ToDateTime(TimeOnly.MinValue).Date;
                DateTime deliveryDate = CurrentOrder.OrderDateReceipt.ToDateTime(TimeOnly.MinValue).Date;

                if (deliveryDate < baseDate)
                {
                    errors.Add($"• Дата доставки не может быть раньше даты заказа ({baseDate:dd.MM.yyyy})");
                }
                else if (deliveryDate > baseDate.AddMonths(1).Date)
                {
                    errors.Add($"• Дата доставки не может быть позже чем на месяц от даты заказа");
                }
            }

            // Доп проверка
            if (CurrentOrder.OrderDateReceipt.ToDateTime(TimeOnly.MinValue) > DateTime.Today.Date && CurrentOrder.OrderStatus == "Завершён")
            {
                errors.Add("• Товар не может быть завершён, если дата доставки не настала");
            }

            // Позиции заказа
            if (_positions.Count == 0)
                errors.Add("• В заказе должен быть хотя бы один товар");
            else
            {
                for (int i = 0; i < _positions.Count; i++)
                {
                    var pos = _positions[i];
                    if (pos.SelectedProduct == null)
                        errors.Add($"• Позиция #{i + 1}: выберите товар");
                    else if (string.IsNullOrWhiteSpace(pos.ProductArticle))
                        errors.Add($"• Позиция #{i + 1}: товар не имеет артикула");
                    else if (pos.Quantity <= 0)
                        errors.Add($"• Позиция #{i + 1}: количество должно быть больше 0");
                    else if (pos.Quantity > 999)
                        errors.Add($"• Позиция #{i + 1}: максимальное количество — 999 шт.");
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
                    if (IsOrderNew)
                    {
                        CurrentOrder.UserId = UserContext.CurrentUser.UserId;
                        context.Orders.Add(CurrentOrder);
                        context.SaveChanges();

                        foreach (var pos in _positions)
                        {
                            context.OrderPositions.Add(new OrderPosition
                            {
                                OrderId = CurrentOrder.OrderId,
                                ProductArticle = pos.ProductArticle,
                                ProductQuantity = pos.Quantity
                            });
                        }
                    }
                    else
                    {
                        var dbOrder = context.Orders
                            .Include(o => o.OrderPositions)
                            .First(o => o.OrderId == CurrentOrder.OrderId);

                        dbOrder.OrderDateMake = CurrentOrder.OrderDateMake;
                        dbOrder.OrderDateReceipt = CurrentOrder.OrderDateReceipt;
                        dbOrder.PickUpPointId = CurrentOrder.PickUpPointId;
                        dbOrder.OrderStatus = CurrentOrder.OrderStatus;

                        context.OrderPositions.RemoveRange(dbOrder.OrderPositions);
                        dbOrder.OrderPositions.Clear();

                        foreach (var pos in _positions)
                        {
                            dbOrder.OrderPositions.Add(new OrderPosition
                            {
                                ProductArticle = pos.ProductArticle,
                                ProductQuantity = pos.Quantity
                            });
                        }
                    }

                    context.SaveChanges();
                }

                MessageBox.Show(IsOrderNew
                    ? "Заказ успешно создан!"
                    : "Заказ успешно обновлён!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                this.Close();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("foreign key") == true)
            {
                MessageBox.Show("Ошибка целостности данных: выбранный продукт или пункт выдачи был удалён из базы.\n" +
                               "Пожалуйста, обновите список и выберите корректные значения.",
                               "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа:\n{ex.Message}",
                               "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void orderDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDeliveryDatePickerLimits();
        }
    }
}
