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
        public List<Кабинет> Cabinetlist;
        public List<Подразделение> Divisionlist;
        public List<Должность> Joblist;
        public EmployeesViewModel ViewModel { get; } = new EmployeesViewModel();
        public Employees()
        {
            InitializeComponent();
            List<Сотрудник> equipments;
            using (dbMalukovEntities db = new dbMalukovEntities())
            {
                equipments = db.Сотрудник.ToList();
                Cabinetlist = db.Кабинет.OrderBy(f => f.Название).ToList();
                Cabinetcmb.ItemsSource = Cabinetlist;
                Cabinetcmb.DisplayMemberPath = "Название";
                Cabinetcmb.SelectedValuePath = "Номер_кабинета";

                Divisionlist = db.Подразделение.OrderBy(f => f.Подразделение1).ToList();
                Divisioncmb.ItemsSource = Divisionlist;
                Divisioncmb.DisplayMemberPath = "Подразделение1";
                Divisioncmb.SelectedValuePath = "Код_подразделения";

                Joblist = db.Должность.OrderBy(f => f.Должность1).ToList();
                Jobcmb.ItemsSource = Joblist;
                Jobcmb.DisplayMemberPath = "Должность1";
                Jobcmb.SelectedValuePath = "Код_должности";
            }
            foreach (var item in equipments)
            {
                ViewModel.Equipments.Add(item);
            }

            if (AuthInfoAbout.Auth != 1) // ограничения для пользователя
            {
                BtnMode.IsEnabled = false;
                BtnCancel.IsEnabled = false;
                ActivateGroupBoxAdd.IsEnabled = false;
                Delete.IsEnabled = false;
                ActivateGroupBoxEdit.IsEnabled = false;
                Cabinetcmb.IsEnabled = false;
                Divisioncmb.IsEnabled = false;
                Jobcmb.IsEnabled = false;
                NameTxb.IsEnabled = false;
                PhoneTxb.IsEnabled = false;
                datapicker.IsEnabled = false;

            }
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

        private void BtnMode_Click(object sender, RoutedEventArgs e) // switcher
        {
            if (ViewModel.Mode == ViewMode.Add) // add
            {
                if (!string.IsNullOrEmpty(Cabinetcmb.Text) && !string.IsNullOrEmpty(Divisioncmb.Text)
                    && !string.IsNullOrEmpty(Jobcmb.Text) && !string.IsNullOrEmpty(NameTxb.Text))
                {
                    using (dbMalukovEntities db = new dbMalukovEntities())
                    {
                        db.Сотрудник.Add(ViewModel.EditableEquipment);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Новый сотрудник успешно добавлен!");
                    ViewModel.Equipments.Add(ViewModel.EditableEquipment);
                    ViewModel.Mode = ViewMode.View;
                }
                else
                {
                    MessageBox.Show("Не заполнены поля: ФИО, Кабинет, Подразделение, Должность", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (ViewModel.Mode == ViewMode.Edit) // edit
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    var equipment = db.Сотрудник.FirstOrDefault(eq => eq.Код_сотр == ViewModel.EditableEquipment.Код_сотр);
                    equipment.Код_сотр = ViewModel.EditableEquipment.Код_сотр;
                    equipment.ФИО_сотр = ViewModel.EditableEquipment.ФИО_сотр;
                    equipment.Дата_рождения = ViewModel.EditableEquipment.Дата_рождения;
                    equipment.Номер_кабинета = ViewModel.EditableEquipment.Номер_кабинета;
                    equipment.Код_подразделения = ViewModel.EditableEquipment.Код_подразделения;
                    equipment.Код_должности = ViewModel.EditableEquipment.Код_должности;
                    equipment.Номер_телефона = ViewModel.EditableEquipment.Номер_телефона;
                    equipment.Должность = ViewModel.EditableEquipment.Должность;
                    equipment.Выдача = ViewModel.EditableEquipment.Выдача;
                    equipment.Подразделение = ViewModel.EditableEquipment.Подразделение;
                    db.SaveChanges();
                    ViewModel.EditableEquipment = equipment;
                }
                MessageBox.Show("Информация обновлена!");
                for (int i = 0; i < ViewModel.Equipments.Count; i++)
                {
                    if (ViewModel.Equipments[i].Код_сотр == ViewModel.EditableEquipment.Код_сотр)
                    {
                        ViewModel.Equipments[i] = ViewModel.EditableEquipment;
                    }
                }
                ViewModel.EditableEquipment = null;
                ViewModel.Mode = ViewMode.View;
            }
        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Mode = ViewMode.View;
        }

        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Сотрудник();
        }

        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // добавление
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
                Номер_телефона = ViewModel.SelectedEquipment.Номер_телефона
            };
        }

        private void DeleteStringFromGrid_Click(object sender, RoutedEventArgs e)
        {
            int Id = (EmployeesGrid.SelectedItem as Сотрудник).Код_сотр;
            var db = new dbMalukovEntities();
            var deleteEmployee = db.Сотрудник.Where(m => m.Код_сотр == Id).Single();
            db.Сотрудник.Remove(deleteEmployee);
            db.SaveChanges();
            EmployeesGrid.ItemsSource = db.Сотрудник.ToList();
            MessageBox.Show("Удаление успешно");
        }

        private void EmployeesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                Номер_телефона = ViewModel.SelectedEquipment.Номер_телефона
            };
        }
    }
}
