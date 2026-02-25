using ShoeAccounting.Models;
using ShoeAccounting.Utils;
using ShoeAccounting.Views.Windows;
using System.Windows;
using static ShoeAccounting.Controllers.UserController;

namespace ShoeAccounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();

            string? errorMessage = ValidateLoginData(login, password);
            if (errorMessage != null)
                MessageBox.Show(errorMessage, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                User? user = LoginUser(login, password);
                if (user == null)
                    MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    UserContext.CurrentUser = user;
                    MessageBox.Show("Добро пожаловать!", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigateToCatalog();
                }
            }

        }

        private string? ValidateLoginData(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                return "Логин не может быть пустым";

            else if (string.IsNullOrEmpty(password))
                return "Пароль не может быть пустым";

            return null;
        }

        private void NavigateToCatalog()
        {
            new CatalogWindow().Show();
            Close();
        }

        private void catalogButton_Click(object sender, RoutedEventArgs e)
        {
            UserContext.CurrentUser = new User()
            {
                UserLastname = "Режим",
                UserFirstname = "гостя",
                UserRole = "Гость"
            };

            NavigateToCatalog();
        }
    }
}