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
        public MaterialLiabilityVM ViewModel { get; } = new MaterialLiabilityVM();


        private readonly CollectionViewSource filteremployee;

        public List<Сотрудник> Employeelist;
        public MaterialLiability()
        {
            InitializeComponent();

            ViewModel = (MaterialLiabilityVM)Resources["vm"];
            filteremployee = (CollectionViewSource)Resources["filteremployee"];
        }
        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // добавление
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.РедактируемаяВыдача = new Выдача();
        }
        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // редактирование
        {
            ViewModel.Mode = ViewMode.Edit;
            BtnMode.Content = "Редактировать";
            ViewModel.РедактируемаяВыдача = new Выдача()
            {
                Код_сотр = ViewModel.SelectedEquipment.Код_сотр,
                Код_техники = ViewModel.SelectedEquipment.Код_техники,
                Дата_выдачи = ViewModel.SelectedEquipment.Дата_выдачи,
                Дата_окончания = ViewModel.SelectedEquipment.Дата_окончания,
                Эксплуатация = ViewModel.SelectedEquipment.Эксплуатация,
                ID = ViewModel.SelectedEquipment.ID,
                Сотрудник = ViewModel.SelectedEquipment.Сотрудник,
                Техника = ViewModel.SelectedEquipment.Техника
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
        //private void Update()
        //{
        //    try
        //    {
        //        var db = new dbMalukovEntities();
        //        var us = db.Выдача.ToList();
        //        var sotr = (cmbSearch.SelectedItem as Сотрудник).Код_сотр;
        //        if (cmbSearch.SelectedIndex != 0)
        //        {
        //            us = us.Where(f => f.Код_сотр == sotr).ToList(); // фильтр по фамилиям сотрудников
        //        }
        //        if (ViewModel.ЕслиЭксплуатация)
        //        {
        //            us = us.Where(f => f.Эксплуатация == true).ToList(); // фильтр по эксплуатации
        //        }
        //        if (txbSearch.Text.Length > 0)
        //        {
        //            us = us.Where(find => find.Техника.Название.Contains(txbSearch.Text)).ToList(); // фильтр по эксплуатации
        //        }
        //        filteremployee?.View.Refresh();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Одно из полей поиска не заполнено");
        //    }
        //}
        private void ResetOut()
        {
            CheckedRunning.IsChecked = false;
            ResponsobilityGrid.ItemsSource = DB.Connection.Выдача.ToList();
            txbSearch.Clear();
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск по названию
        {


            filteremployee?.View.Refresh();
            //if (txbSearch.Text.Length > 0)
            //{
            //    Update();
            //}
            //else
            //{
            //    ResetOut();
            //}
        }
        private void CheckRunning_Click(object sender, RoutedEventArgs e) // клик по чекбоксу Эксплуатация
        {
            filteremployee?.View.Refresh();
        }
        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) // фильтр по сотруднику
        {
            filteremployee?.View.Refresh();
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Mode == ViewMode.Add)
            {



                //using (dbMalukovEntities db = new dbMalukovEntities())
                //{
                DB.Connection.Выдача.Add(ViewModel.РедактируемаяВыдача);
                DB.Connection.SaveChanges();
                //}
                MessageBox.Show("Техника успешно выдана сотруднику!");
                ViewModel.Выдачи.Add(ViewModel.РедактируемаяВыдача);
            }
            else if (ViewModel.Mode == ViewMode.Edit) // не работает
            {
                int index = 0;
                for (; index < ViewModel.Выдачи.Count; index++)
                {
                    if (ViewModel.Выдачи[index].ID == ViewModel.РедактируемаяВыдача.ID)
                    {
                        break;
                    }
                }
                if (index < ViewModel.Выдачи.Count)
                {
                    var выдача = ViewModel.Выдачи[index];

                    выдача.Код_сотр = ViewModel.РедактируемаяВыдача.Код_сотр;
                    выдача.Код_техники = ViewModel.РедактируемаяВыдача.Код_техники;
                    выдача.ID = ViewModel.РедактируемаяВыдача.ID;
                    выдача.Дата_выдачи = ViewModel.РедактируемаяВыдача.Дата_выдачи;
                    выдача.Дата_окончания = ViewModel.РедактируемаяВыдача.Дата_окончания;
                    выдача.Эксплуатация = ViewModel.РедактируемаяВыдача.Эксплуатация;
                    выдача.Сотрудник = ViewModel.РедактируемаяВыдача.Сотрудник;
                    выдача.Техника = ViewModel.РедактируемаяВыдача.Техника;
                    DB.Connection.SaveChanges();

                    ViewModel.Выдачи[index] = ViewModel.РедактируемаяВыдача;

                    ViewModel.Выдачи[index] = выдача;
                    MessageBox.Show("Информация обновлена!");
                    ViewModel.РедактируемаяВыдача = null;
                }

                ViewModel.Mode = ViewMode.View;
            }
        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Mode = ViewMode.View;
        }

        private void OnFilterВыдачи(object sender, FilterEventArgs e)
        {
            Выдача выдача = (Выдача)e.Item;

            if (ViewModel.ВыбранныйСотрудник != DB.ВсеСотрудники && ViewModel.ВыбранныйСотрудник != null)
            {
                e.Accepted = выдача.Код_сотр == ViewModel.ВыбранныйСотрудник.Код_сотр;
            }
            if (ViewModel.ЕслиЭксплуатация)
            {
                e.Accepted &= выдача.Эксплуатация;
            }
            if (!string.IsNullOrEmpty(ViewModel.ПоискТехники))
            {
                e.Accepted &= выдача.Техника.Название.Contains(ViewModel.ПоискТехники);
            }
        }
    }

}
