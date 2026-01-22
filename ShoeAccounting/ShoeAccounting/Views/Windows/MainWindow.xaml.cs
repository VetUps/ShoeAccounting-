using ShoeAccounting.Models;
using ShoeAccounting.Views.Windows;
using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        }

        private string? CheckLoginData()
        {
            string loginText = loginTextBox.Text.Trim();
            string passwordText = passwordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(loginText))
                return "Логин не может быть пустым";

            else if (string.IsNullOrEmpty(passwordText))
                return "Пароль не может быть пустым";

            using (ShoesDbContext context = new ShoesDbContext())
            {
                User? user = context.Users.FirstOrDefault(u => u.UserLogin == loginText && u.UserPassword == passwordText);

                if (user == null)
                    return "Неверный логин или пароль";
            }

            return null;
        }

        private void catalogButton_Click(object sender, RoutedEventArgs e)
        {
            new CatalogWindow().Show();
            Close();
        }
    }
}