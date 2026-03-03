using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static ShoeAccounting.Controllers.ProductController;
using static ShoeAccounting.Controllers.ProviderController;

namespace ShoeAccounting.Views.Windows
{
    public partial class CatalogWindow : Window, INotifyPropertyChanged
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
                if (value != null)
                {
                    _productsList = value;
                    OnPropertyChanged();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

            ProductsList = LoadProducts(CurrentFilterMethod, CurrentSortMethod, CurrentSearchText);
        }

        private List<string> LoadProvidersFiltrationList()
        {
            List<Provider> providersList = GetProviders();
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
            ProductsList = LoadProducts(CurrentFilterMethod, CurrentSortMethod, CurrentSearchText);
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFilterMethod = (sender as ComboBox).SelectedItem as string;
            ProductsList = LoadProducts(CurrentFilterMethod, CurrentSortMethod, CurrentSearchText);
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrentSearchText = (sender as TextBox).Text.ToLower().Trim();
            ProductsList = LoadProducts(CurrentFilterMethod, CurrentSortMethod, CurrentSearchText);
        }

        private void newProductButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ProductManagment();
            if (editWindow.ShowDialog() == true)
            {
                if (Window.GetWindow(this) is CatalogWindow catalogWindow)
                {
                    LoadProducts(CurrentFilterMethod, CurrentSortMethod, CurrentSearchText);
                }
            }
        }

        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            new OrdersWindow().Show();
            Close();
        }
    }
}
