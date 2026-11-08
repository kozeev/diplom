using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

namespace kursovaya
{
    public partial class WorkloadWindow : Window
    {
        private DataTable workloadTable;
        private SqlDataAdapter adapter;
        private SqlConnection connection;

        public WorkloadWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            connection = AppConnect.GetConnection(); // исправлено

            try
            {
                connection.Open();
                string query = "SELECT ID, Teacher, Course, GroupName, WeeklyLoad FROM Workload";
                adapter = new SqlDataAdapter(query, connection);
                workloadTable = new DataTable();
                adapter.Fill(workloadTable);
                WorkloadDataGrid.ItemsSource = workloadTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string teacher = TeacherFilter.Text.Trim().ToLower();
            string course = CourseFilter.Text.Trim().ToLower();
            string group = GroupFilter.Text.Trim().ToLower();

            var filtered = workloadTable.AsEnumerable().Where(row =>
                row.Field<string>("Teacher").ToLower().Contains(teacher) &&
                row.Field<string>("Course").ToLower().Contains(course) &&
                row.Field<string>("GroupName").ToLower().Contains(group)
            );

            if (filtered.Any())
            {
                WorkloadDataGrid.ItemsSource = filtered.CopyToDataTable().DefaultView;
            }
            else
            {
                WorkloadDataGrid.ItemsSource = null;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWorkloadWindow addWindow = new AddWorkloadWindow();
            addWindow.Owner = this;
            addWindow.ShowDialog();
            LoadData(); // Перезагружаем данные после добавления
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkloadDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int id = Convert.ToInt32(selectedRow["ID"]);

                try
                {
                    using (SqlConnection conn = AppConnect.GetConnection()) // исправлено
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Workload WHERE ID = @ID";
                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }

                    LoadData(); // Обновляем таблицу после удаления
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.");
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkloadDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int id = Convert.ToInt32(selectedRow["ID"]);
                string teacher = selectedRow["Teacher"].ToString();
                string course = selectedRow["Course"].ToString();
                string group = selectedRow["GroupName"].ToString();
                int weeklyLoad = Convert.ToInt32(selectedRow["WeeklyLoad"]);

                // Вызов обновленного конструктора
                EditWorkloadWindow editWindow = new EditWorkloadWindow(id, teacher, course, group, weeklyLoad);
                editWindow.Owner = this;
                editWindow.ShowDialog();

                LoadData(); // Перезагрузка данных после редактирования
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }
        private void ShowTeacherLoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkloadDataGrid.SelectedItem is DataRowView selectedRow)
            {
                string teacher = selectedRow["Teacher"].ToString();
                TeacherLoadWindow window = new TeacherLoadWindow(teacher);
                window.Owner = this;
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите строку с преподавателем.");
            }
        }


        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(workloadTable);
                MessageBox.Show("Изменения успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
        private void ExportToExcel()
        {
            var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Нагрузка");

            // Заголовки
            worksheet.Cell(1, 1).Value = "Преподаватель";
            worksheet.Cell(1, 2).Value = "Дисциплина";
            worksheet.Cell(1, 3).Value = "Группа";
            worksheet.Cell(1, 4).Value = "Нагрузка";

            // Данные
            for (int i = 0; i < workloadTable.Rows.Count; i++)
            {
                var row = workloadTable.Rows[i];
                worksheet.Cell(i + 2, 1).Value = row["Teacher"].ToString();
                worksheet.Cell(i + 2, 2).Value = row["Course"].ToString();
                worksheet.Cell(i + 2, 3).Value = row["GroupName"].ToString();
                worksheet.Cell(i + 2, 4).Value = row["WeeklyLoad"].ToString();
            }

            // Сохранение
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = "WorkloadExport.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                workbook.SaveAs(dialog.FileName);
                MessageBox.Show("Экспорт в Excel выполнен успешно.", "Успех");
            }
        }

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportToExcel(); 
        }
    }
}
