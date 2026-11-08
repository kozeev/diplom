using System;
using System.Data.SqlClient;
using System.Windows;

namespace kursovaya
{
    public partial class EditWorkloadWindow : Window
    {
        private int recordId; // ID записи, которую редактируем

        public EditWorkloadWindow(int id, string teacher, string course, string group, int weeklyLoad)
        {
            InitializeComponent();

            recordId = id;
            TeacherTextBox.Text = teacher;
            CourseTextBox.Text = course;
            GroupTextBox.Text = group;
            WorkloadPerWeekTextBox.Text = weeklyLoad.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = AppConnect.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE Workload 
                        SET Teacher = @Teacher, 
                            Course = @Course, 
                            GroupName = @GroupName, 
                            WeeklyLoad = @WeeklyLoad 
                        WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Teacher", TeacherTextBox.Text);
                        command.Parameters.AddWithValue("@Course", CourseTextBox.Text);
                        command.Parameters.AddWithValue("@GroupName", GroupTextBox.Text);
                        command.Parameters.AddWithValue("@WeeklyLoad", int.Parse(WorkloadPerWeekTextBox.Text));
                        command.Parameters.AddWithValue("@ID", recordId);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
