using System;
using System.Collections.Generic;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Inspector.Pages
{
    /// <summary>
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : Page
    {
        public EmployeesViewModel ViewModel { get; } = new EmployeesViewModel();
        public Employees()
        {
            InitializeComponent();
            List<Сотрудник> equipments;
            using (dbMalukovEntities db = new dbMalukovEntities())
            {
                equipments = db.Сотрудник.ToList();
            }
            foreach (var item in equipments)
            {
                ViewModel.Equipments.Add(item);
            }
        }

        private void ActivateSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var application = new Excel.Application();
            application.Visible = true;
            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = application.Worksheets.Item[1]; //Sheets[1];
            sheet1.Name = "Сотрудники";

            for (int j = 1; j < EmployeesGrid.Columns.Count + 1; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j];
                //sheet1.Columns[j].ColumnWidth = 25;
                myRange.Value2 = EmployeesGrid.Columns[j - 1].Header;
                myRange.Font.Bold = true;
                myRange.Columns.AutoFit();
            }
            for (int i = 1; i <= EmployeesGrid.Columns.Count; i++)
                for (int j = 0; j < EmployeesGrid.Items.Count; j++)
                {
                    TextBlock b = EmployeesGrid.Columns[i - 1].GetCellContent(EmployeesGrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myrange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i];
                    myrange.Value2 = b.Text;
                }
        }
        private void ResetOut()
        {

            EmployeesGrid.ItemsSource = DB.Connection.Сотрудник.ToList();
            txbSearch.Clear();
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var us = db.Сотрудник.ToList();
            IEnumerable<Сотрудник> result = null;
            CompactFiltering.Visibility = Visibility.Visible;
            if (txbSearch.Text.Length > 0)
            {
                result = us.Where(find => find.ФИО_сотр.Contains(txbSearch.Text));
                EmployeesGrid.ItemsSource = result.ToList();
            }
            else
            {
                ResetOut();
            }
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Сотрудник();
        }

        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Mode = ViewMode.Edit;
            BtnMode.Content = "Редактировать";
            ViewModel.EditableEquipment = new Сотрудник()
            {
                Код_сотр = ViewModel.SelectedEquipment.Код_сотр,
                ФИО_сотр = ViewModel.SelectedEquipment.ФИО_сотр,
                Дата_рождения = ViewModel.SelectedEquipment.Дата_рождения,
                Номер_кабинета = ViewModel.SelectedEquipment.Номер_кабинета,
                Код_подразделения = ViewModel.SelectedEquipment.Код_подразделения,
                Код_должности = ViewModel.SelectedEquipment.Код_должности,
                Номер_телефона = ViewModel.SelectedEquipment.Номер_телефона,
                //Выдача = ViewModel.SelectedEquipment.Выдача
            };
        }

        private void DeleteStringFromGrid_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
