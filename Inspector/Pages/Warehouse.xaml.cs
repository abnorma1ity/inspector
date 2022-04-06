using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Warehouse.xaml
    /// </summary>
    public partial class Warehouse : Page
    {
        //public int mode = 0; // режим работы с groupbox 0 - добавление, 1 - редактирование
        dbMalukovEntities db = new dbMalukovEntities();
        public WarehouseViewModel ViewModel { get; } = new WarehouseViewModel();
        public Warehouse()
        {
            List<Техника> equipments;
            using (dbMalukovEntities db = new dbMalukovEntities())
            {
                equipments = db.Техника.ToList();
            }
            foreach (var item in equipments)
            {
                ViewModel.Equipments.Add(item);
            }

            InitializeComponent();
        }

        //public Техника Mytech { get; set; }

        private void ResetOut()
        {

            WarehouseGrid.ItemsSource = DB.Connection.Техника.ToList();
            txbSearch.Clear();
        }

        private void ActivateSearch_Click(object sender, RoutedEventArgs e)
        {

            CompactFiltering.Visibility = Visibility.Visible;

        }

        private void DeactivateSearch_Click(object sender, RoutedEventArgs e)
        {
            CompactFiltering.Visibility = Visibility.Hidden;
            WarehouseGrid.ItemsSource = DB.Connection.Техника.ToList();
        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e)
        {
            //MenuGroupBox.Visibility = Visibility.Hidden;
        }

        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // меню действия добавить
        {
            //MenuGroupBox.Visibility = Visibility.Visible;
            ViewModel.Mode = ViewMode.Add;
            //ChangeModeLabel.Content = "Добавление записи";
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Техника();
            // MessageBox.Show(Mytech.ToString());
        }

        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // меню действия редактировать
        {
            //MenuGroupBox.Visibility = Visibility.Visible;
            ViewModel.Mode = ViewMode.Edit;
            //ChangeModeLabel.Content = "Редактирование записи";
            BtnMode.Content = "Редактировать";
            ViewModel.EditableEquipment = new Техника()
            {
                Название = ViewModel.SelectedEquipment.Название,
                Модель = ViewModel.SelectedEquipment.Модель,
                Параметры = ViewModel.SelectedEquipment.Параметры,
                Остаток_на_складе = ViewModel.SelectedEquipment.Остаток_на_складе,
                Цена = ViewModel.SelectedEquipment.Цена,
                Дата_последнего_обновления = ViewModel.SelectedEquipment.Дата_последнего_обновления,
                Код = ViewModel.SelectedEquipment.Код,
                Количество = ViewModel.SelectedEquipment.Количество,
                //Выдача = ViewModel.SelectedEquipment.Выдача
            };
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e) // кнопка изменения режима (добавить или рещдактировать)
        {
            /* var name = NameTxb.Text;
             var model = ModelTxb.Text;
             var parametrs = ParamsTxb.Text;
             var ostatok = OstatokTxb.Text;
             var price = PriceTxb.Text;
             var dtPicker = datapicker.DisplayDate;
             var u = new Техника();

             u.Название = name;
             u.Модель = model;
             u.Параметры = parametrs;
             u.Остаток_на_складе = Convert.ToInt32(ostatok);
             u.Цена = Convert.ToInt32(price);
             u.Дата_последнего_обновления = dtPicker; */

            /*if (string.IsNullOrEmpty(NameTxb.Text) == true || string.IsNullOrEmpty(ModelTxb.Text) == true || string.IsNullOrEmpty(ParamsTxb.Text) == true
                || string.IsNullOrEmpty(OstatokTxb.Text) == true || string.IsNullOrEmpty(PriceTxb.Text) == true)
            {
                MessageBox.Show("Не все данные введены!");
                return;
            } */
            //MessageBox.Show("***" + Mytech.ToString());
            if (ViewModel.Mode == ViewMode.Add)
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    db.Техника.Add(ViewModel.EditableEquipment);
                    db.SaveChanges();
                }
                MessageBox.Show("Новая техника зарегистрирована на складе!");
                ViewModel.Equipments.Add(ViewModel.EditableEquipment);
            }
            else if (ViewModel.Mode == ViewMode.Edit) // не работает
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    var equipment = db.Техника.FirstOrDefault(eq => eq.Код == ViewModel.EditableEquipment.Код);
                    equipment.Название = ViewModel.EditableEquipment.Название;
                    equipment.Модель = ViewModel.EditableEquipment.Модель;
                    equipment.Параметры = ViewModel.EditableEquipment.Параметры;
                    equipment.Остаток_на_складе = ViewModel.EditableEquipment.Остаток_на_складе;
                    equipment.Цена = ViewModel.EditableEquipment.Цена;
                    equipment.Дата_последнего_обновления = ViewModel.EditableEquipment.Дата_последнего_обновления;
                    equipment.Код = ViewModel.EditableEquipment.Код;
                    equipment.Количество = ViewModel.EditableEquipment.Количество;
                    equipment.Выдача = ViewModel.EditableEquipment.Выдача;
                    db.SaveChanges();
                    ViewModel.EditableEquipment = equipment;
                }
                MessageBox.Show("Информация обновлена!");
                for (int i = 0; i < ViewModel.Equipments.Count; i++)
                {
                    if(ViewModel.Equipments[i].Код == ViewModel.EditableEquipment.Код)
                    {
                        ViewModel.Equipments[i] = ViewModel.EditableEquipment;
                    }
                }
                ViewModel.EditableEquipment = null;
            }
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск сломался
        {
             if (CheckStr.IsChecked == true)
            {
                var db = new dbMalukovEntities();
                var us = db.Техника.ToList();
                CompactFiltering.Visibility = Visibility.Visible;
                if (txbSearch.Text.Length > 0)
                {
                    var result = us.Where(f => f.Название.Contains(txbSearch.Text));
                    WarehouseGrid.ItemsSource = result.ToList();
                }
                else
                {
                    ResetOut();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // кнопка ОК поиска
        {
            CheckStr.IsChecked = false;
            if (CheckedDate.IsChecked == true)
            {
                var date1 = dtFirst.SelectedDate;
                var date2 = dtSecond.SelectedDate;
                var db = new dbMalukovEntities();
                var us = db.Техника.ToList();
                IEnumerable<Техника> result = null;
                result = us.Where(find => find.Дата_последнего_обновления >= date1 & find.Дата_последнего_обновления <= date2);
                WarehouseGrid.ItemsSource = result.ToList();
            }
        }


        private void CheckStr_Click(object sender, RoutedEventArgs e) // поиск по текстбоксу
        {
            CheckedDate.IsChecked = false;
            txbSearch.IsEnabled = true;
            btnOK.IsEnabled = false;
            WarehouseGrid.ItemsSource = DB.Connection.Техника.ToList();
            txbSearch.Focus();
            txbSearch.Clear();
        }

        private void CheckedDate_Click(object sender, RoutedEventArgs e) // поиск по дате
        {
            CheckStr.IsChecked = false;
            txbSearch.IsEnabled = false;
            btnOK.IsEnabled = true;
            txbSearch.Clear();
            WarehouseGrid.ItemsSource = DB.Connection.Техника.ToList();
        }

        private void Export_Click(object sender, RoutedEventArgs e) // экспорт
        {
            var db = new dbMalukovEntities();
            var application = new Excel.Application();
            application.Visible = true;
            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = application.Worksheets.Item[1]; //Sheets[1];
            sheet1.Name = "Склад техники";

            for (int j = 1; j < WarehouseGrid.Columns.Count + 1; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j];
                //sheet1.Columns[j].ColumnWidth = 25;
                myRange.Value2 = WarehouseGrid.Columns[j - 1].Header;
                myRange.Font.Bold = true;
            }
            for (int i = 1; i <= WarehouseGrid.Columns.Count; i++)
                for (int j = 0; j < WarehouseGrid.Items.Count; j++)
                {
                    TextBlock b = WarehouseGrid.Columns[i - 1].GetCellContent(WarehouseGrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myrange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i];
                    myrange.Value2 = b.Text;
                }


        }
        private void DeleteStringFromGrid_Click(object sender, RoutedEventArgs e) // удаление
        {
            int Id = (WarehouseGrid.SelectedItem as Техника).Код;
            var deleteTech = db.Техника.Where(m => m.Код == Id).Single();
            db.Техника.Remove(deleteTech);
            db.SaveChanges();
            WarehouseGrid.ItemsSource = db.Техника.ToList();
            MessageBox.Show("Удаление успешно");
        }

    }
}
