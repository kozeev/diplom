using System;
using System.Data.SqlClient;
using System.Windows;

namespace kursovaya
{
    public partial class AddWorkloadWindow : Window
    {
        public AddWorkloadWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string teacher = TeacherTextBox.Text.Trim();
            string course = CourseTextBox.Text.Trim();
            string group = GroupTextBox.Text.Trim();
            string weeklyLoadStr = WeeklyLoadTextBox.Text.Trim();

            if (string.IsNullOrEmpty(teacher) || string.IsNullOrEmpty(course) ||
                string.IsNullOrEmpty(group) || string.IsNullOrEmpty(weeklyLoadStr))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
                return;
            }

            if (!int.TryParse(weeklyLoadStr, out int weeklyLoad))
            {
                MessageBox.Show("Нагрузка должна быть числом!");
                return;
            }

            try
            {
                using (SqlConnection conn = AppConnect.GetConnection())
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO Workload (Teacher, Course, GroupName, WeeklyLoad) " +
                                         "VALUES (@Teacher, @Course, @GroupName, @WeeklyLoad)";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@Teacher", teacher);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.Parameters.AddWithValue("@GroupName", group);
                    cmd.Parameters.AddWithValue("@WeeklyLoad", weeklyLoad);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Нагрузка успешно добавлена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }
    }
}
