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
        public List<Сотрудник> list;
        public MaterialLiability()
        {
            InitializeComponent();
            var db = new dbMalukovEntities();
            ResponsobilityGrid.ItemsSource = db.Выдача.ToList();
            list = db.Сотрудник.OrderBy(f => f.ФИО_сотр).ToList();
            list.Insert(0, new Сотрудник
            {
                ФИО_сотр = "Все сотрудники"
            });
            cmbSearch.ItemsSource = list;
            cmbSearch.DisplayMemberPath = "ФИО_сотр";
            cmbSearch.SelectedValuePath = "Код_сотр";
            this.DataContext = this;
        }
        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // добавление
        {
            NavigationService.Navigate(new AddMaterialLiabilityPage(new Выдача()));
        }
        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // редактирование
        {
            var u = ResponsobilityGrid.SelectedItem as Выдача; 
            DB.Connection.SaveChanges();
            MessageBox.Show("Изменение успешно");
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
        private void ActivateSearch_Click(object sender, RoutedEventArgs e)
        {
            CompactFiltering.Visibility = Visibility.Visible;
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
    }
}
