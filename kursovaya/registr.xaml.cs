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
using System.Windows.Shapes;

namespace kursovaya
{
    /// <summary>
    /// Логика взаимодействия для registr.xaml
    /// </summary>
    public partial class registr : Window
    {
        public registr()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
           string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;

            try
            {
                using (SqlConnection connection = AppConnect.GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO Users (Username, PasswordHash, FirstName, LastName) VALUES (@Username, @PasswordHash, @FirstName, @LastName)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", password); // Лучше хранить пароли в виде хэшей
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        MessageBox.Show("Регистрация выполнена успешно!");
                        // Здесь можно добавить код для перехода на другое окно или выполнения других действий
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при регистрации пользователя!");
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
            MainWindow mainWindow = new MainWindow();

            
            mainWindow.Show();

           
            this.Close();
        }
    }
}
