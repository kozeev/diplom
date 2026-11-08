using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace kursovaya
{
    public partial class TeacherLoadWindow : Window
    {
        private string teacherName;

        public TeacherLoadWindow(string teacher)
        {
            InitializeComponent();
            teacherName = teacher;
            LoadTeacherData();
        }

        private void LoadTeacherData()
        {
            try
            {
                using (SqlConnection conn = AppConnect.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT Course, GroupName, WeeklyLoad
                        FROM Workload
                        WHERE Teacher = @Teacher";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Teacher", teacherName);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    TeacherLoadDataGrid.ItemsSource = table.DefaultView;

                    // Подсчет общей нагрузки
                    int total = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        total += Convert.ToInt32(row["WeeklyLoad"]);
                    }

                    TotalLoadText.Text = $"Суммарная нагрузка: {total} ч";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }
    }
}
