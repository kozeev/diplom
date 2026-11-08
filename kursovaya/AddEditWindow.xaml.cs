using System.Windows;

namespace kursovaya
{
    public partial class AddEditWindow : Window
    {
        public string Teacher { get; private set; }
        public string Subject { get; private set; }
        public string Group { get; private set; }
        public string PlanHours { get; private set; }
        public string FactHours { get; private set; }
        public string LessonType { get; private set; }

        public AddEditWindow(string teacher = "", string subject = "", string group = "", string plan = "", string fact = "", string type = "")
        {
            InitializeComponent();
            TeacherBox.Text = teacher;
            SubjectBox.Text = subject;
            GroupBox.Text = group;
            PlanHoursBox.Text = plan;
            FactHoursBox.Text = fact;
            TypeComboBox.Text = type;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Teacher = TeacherBox.Text;
            Subject = SubjectBox.Text;
            Group = GroupBox.Text;
            PlanHours = PlanHoursBox.Text;
            FactHours = FactHoursBox.Text;
            LessonType = TypeComboBox.Text;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
