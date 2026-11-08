using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursovaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            try
            {
                using (SqlConnection connection = AppConnect.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", password); // Лучше хранить пароли в виде хэшей и сравнивать хэши

                    int count = (int)command.ExecuteScalar();

                    if (count == 1)
                    {
                        MessageBox.Show("Вход выполнен успешно!");
                        UserCabinet userCabinet = new UserCabinet();
                        userCabinet.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильное имя пользователя или пароль!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new registr();
            registrationWindow.Show();
            this.Close();
        }
    }
}
