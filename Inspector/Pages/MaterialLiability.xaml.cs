using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для MaterialLiability.xaml
    /// </summary>
    public partial class MaterialLiability : Page
    {
        public MaterialLiabilityViewModel ViewModel { get; } = new MaterialLiabilityViewModel();
        public List<Сотрудник> Employeelist;
        public List<Сотрудник> Employeeslist;
        public List<Техника> Techlist;
        public MaterialLiability()
        {
            InitializeComponent();
            List<Выдача> equipments;
            using (dbMalukovEntities db = new dbMalukovEntities())
            {
                equipments = db.Выдача.ToList();
                Employeelist = db.Сотрудник.OrderBy(f => f.ФИО_сотр).ToList();
                Employeelist.Insert(0, new Сотрудник
                {
                    ФИО_сотр = "Все сотрудники"
                });
                cmbSearch.ItemsSource = Employeelist;
                cmbSearch.DisplayMemberPath = "ФИО_сотр";
                cmbSearch.SelectedValuePath = "Код_сотр";

                Techcmb.ItemsSource = Techlist;
                Techcmb.DisplayMemberPath = "Название";
                Techcmb.SelectedValuePath = "Код_техники";

                Employeecmb.ItemsSource = Employeeslist;
                Employeecmb.DisplayMemberPath = "ФИО_сотр";
                Employeecmb.SelectedValuePath = "Код_сотр";
            }
            foreach (var item in equipments)
            {
                ViewModel.Equipments.Add(item);
            }

        }
        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // добавление
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Выдача();
        }
        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // редактирование
        {
            ViewModel.Mode = ViewMode.Edit;
            BtnMode.Content = "Редактировать";
            ViewModel.EditableEquipment = new Выдача()
            {
                Код_сотр = ViewModel.SelectedEquipment.Код_сотр,
                Код_техники = ViewModel.SelectedEquipment.Код_техники,
                Дата_выдачи = ViewModel.SelectedEquipment.Дата_выдачи,
                Дата_окончания = ViewModel.SelectedEquipment.Дата_окончания,
                Эксплуатация = ViewModel.SelectedEquipment.Эксплуатация,
                ID = ViewModel.SelectedEquipment.ID,
                //Выдача = ViewModel.SelectedEquipment.Выдача
            };
        }
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var application = new Excel.Application();
            application.Visible = true;
            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = application.Worksheets.Item[1]; //Sheets[1];
            sheet1.Name = "Выдача";

            for (int j = 1; j < ResponsobilityGrid.Columns.Count + 1; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j];
                //sheet1.Columns[j].ColumnWidth = 25;
                myRange.Value2 = ResponsobilityGrid.Columns[j - 1].Header;
                myRange.Font.Bold = true;
                myRange.Columns.AutoFit();
            }
            for (int i = 1; i <= ResponsobilityGrid.Columns.Count; i++)
                for (int j = 0; j < ResponsobilityGrid.Items.Count; j++)
                {
                    TextBlock b = ResponsobilityGrid.Columns[i - 1].GetCellContent(ResponsobilityGrid.Items[j]) as TextBlock;
                    if (b == null)
                        continue;

                    Microsoft.Office.Interop.Excel.Range myrange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i];
                    myrange.Value2 = b.Text;
                }

        }
        private void Update()
        {
            try
            {
                var db = new dbMalukovEntities();
                var us = db.Выдача.ToList();
                var sotr = (cmbSearch.SelectedItem as Сотрудник).Код_сотр;
                if (cmbSearch.SelectedIndex != 0)
                {
                    us = us.Where(f => f.Код_сотр == sotr).ToList(); // фильтр по фамилиям сотрудников
                }
                if (CheckedRunning.IsChecked == true)
                {
                    us = us.Where(f => f.Эксплуатация == true).ToList(); // фильтр по эксплуатации
                }
                if (txbSearch.Text.Length > 0)
                {
                    us = us.Where(find => find.Техника.Название.Contains(txbSearch.Text)).ToList(); // фильтр по эксплуатации
                }
                ResponsobilityGrid.ItemsSource = us.ToList();
            }
            catch
            {
                MessageBox.Show("Одно из полей поиска не заполнено");
            }
        }
        private void ResetOut()
        {
            CheckedRunning.IsChecked = false;
            ResponsobilityGrid.ItemsSource = DB.Connection.Выдача.ToList();
            txbSearch.Clear();
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск по названию
        {
            if (txbSearch.Text.Length > 0)
            {
                Update();
            }
            else
            {
                ResetOut();
            }
        }
        private void CheckRunning_Click(object sender, RoutedEventArgs e) // клик по чекбоксу Эксплуатация
        {
            Update();
        }
        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) // фильтр по сотруднику
        {
            Update();
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Mode == ViewMode.Add)
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    db.Выдача.Add(ViewModel.EditableEquipment);
                    db.SaveChanges();
                }
                MessageBox.Show("Техника успешно выдана сотруднику!");
                ViewModel.Equipments.Add(ViewModel.EditableEquipment);
            }
            else if (ViewModel.Mode == ViewMode.Edit) // не работает
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    var equipment = db.Выдача.FirstOrDefault(eq => eq.ID == ViewModel.EditableEquipment.ID);
                    equipment.Код_сотр = ViewModel.EditableEquipment.Код_сотр;
                    equipment.Код_техники = ViewModel.EditableEquipment.Код_техники;
                    equipment.ID = ViewModel.EditableEquipment.ID;
                    equipment.Дата_выдачи = ViewModel.EditableEquipment.Дата_выдачи;
                    equipment.Дата_окончания = ViewModel.EditableEquipment.Дата_окончания;
                    equipment.Эксплуатация = ViewModel.EditableEquipment.Эксплуатация;
                    equipment.Сотрудник = ViewModel.EditableEquipment.Сотрудник;
                    equipment.Техника = ViewModel.EditableEquipment.Техника;
                    db.SaveChanges();
                    ViewModel.EditableEquipment = equipment;
                }
                MessageBox.Show("Информация обновлена!");
                for (int i = 0; i < ViewModel.Equipments.Count; i++)
                {
                    if (ViewModel.Equipments[i].ID == ViewModel.EditableEquipment.ID)
                    {
                        ViewModel.Equipments[i] = ViewModel.EditableEquipment;
                    }
                }
                ViewModel.EditableEquipment = null;
            }
        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
