using Microsoft.EntityFrameworkCore;
using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public DateTime? OrderDateMakeDateTime
        {
            get => CurrentOrder.OrderDateMake?.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentOrder.OrderDateMake = value.HasValue
                ? DateOnly.FromDateTime(value.Value.Date)
                : null;
                OnPropertyChanged();
            }
        }

        public DateTime? OrderDateReceiptDateTime
        {
            get => CurrentOrder.OrderDateReceipt?.ToDateTime(TimeOnly.MinValue);
            set
            {
                CurrentOrder.OrderDateReceipt = value.HasValue
                ? DateOnly.FromDateTime(value.Value.Date)
                : null;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName="")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public OrderManagmentWindow(Order? order = null)
        {
            InitializeComponent();

            if (order == null)
            {
                CurrentOrder = new Order();
                IsOrderNew = true;
            }
            else
            {
                CurrentOrder = order;
                IsOrderNew = false;
            }

            LoadComboBoxData();
            SetOrderDatePickerLimits();
            SetDeliveryDatePickerLimits();

            DataContext = this;
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
            if (OrderDateReceiptDateTime?.Date < baseDate ||
                OrderDateReceiptDateTime?.Date > baseDate.AddMonths(1).Date)
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

            // Артикул продукта
            if (string.IsNullOrWhiteSpace(CurrentOrder.ProductArticle))
            {
                errors.Add("• Выберите продукт из списка");
            }
            else if (!Products.Any(p => p.ProductArticle == CurrentOrder.ProductArticle))
            {
                errors.Add("• Выбранный продукт не существует в базе данных");
            }

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

            // Дата доставки заказа
            if (!CurrentOrder.OrderDateMake.HasValue)
            {
                errors.Add("• Укажите дату заказа");
            }

            // Дата доставки
            if (!CurrentOrder.OrderDateReceipt.HasValue)
            {
                errors.Add("• Укажите дату доставки");
            }
            else if (CurrentOrder.OrderDateReceipt.Value.ToDateTime(TimeOnly.MinValue) < DateTime.Today.Date)
            { 
                errors.Add("• Дата доставки не может быть в прошлом");
            }
            else if (CurrentOrder.OrderDateMake.HasValue)
            {
                DateTime baseDate = CurrentOrder.OrderDateMake.Value.ToDateTime(TimeOnly.MinValue).Date;
                DateTime deliveryDate = CurrentOrder.OrderDateReceipt.Value.ToDateTime(TimeOnly.MinValue).Date;

                if (deliveryDate < baseDate)
                {
                    errors.Add($"• Дата доставки не может быть раньше даты заказа ({baseDate:dd.MM.yyyy})");
                }
                else if (deliveryDate > baseDate.AddMonths(1).Date)
                {
                    errors.Add($"• Дата доставки не может быть позже чем на месяц от даты заказа");
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
                        CurrentOrder.OrderDateMake = DateOnly.FromDateTime(DateTime.Today.Date);
                        context.Orders.Add(CurrentOrder);
                    }
                    else
                    {
                        context.Orders.Attach(CurrentOrder);
                        context.Entry(CurrentOrder).State = EntityState.Modified;
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
