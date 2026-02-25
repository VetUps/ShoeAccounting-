using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using ShoeAccounting.Views.Windows;
using System.Windows;
using System.Windows.Controls;
using static ShoeAccounting.Controllers.ProductController;
using static ShoeAccounting.Controllers.OrderController;

namespace ShoeAccounting.Views.Controll
{
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
                    catalogWindow.ProductsList = LoadProducts(catalogWindow.CurrentFilterMethod, catalogWindow.CurrentSortMethod, catalogWindow.CurrentSearchText);
                }
            }
        }

        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product currentProduct = DataContext as Product;

            OrderPosition? orderPosition = GetOrderPositionByProduct(currentProduct);
            if (orderPosition != null)
            {
                MessageBox.Show("Товар нельзя удалить, так как он есть в заказе", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DeleteProduct(currentProduct);

                    if (Window.GetWindow(this) is CatalogWindow catalogWindow)
                    {
                        catalogWindow.ProductsList = LoadProducts(catalogWindow.CurrentFilterMethod, catalogWindow.CurrentSortMethod, catalogWindow.CurrentSearchText);
                    }
                }

            }
        }
    }
}
