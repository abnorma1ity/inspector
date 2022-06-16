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
using static Inspector.Pages.ExcelHelper;

namespace Inspector.Pages
{
    /// <summary>
    /// Логика взаимодействия для Warehouse.xaml
    /// </summary>
    public partial class Warehouse : Page
    {
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
            if (AuthInfoAbout.Auth != 1) // ограничения пользователя
            {
                BtnMode.IsEnabled = false;
                BtnCancel.IsEnabled = false;
                ActivateGroupBoxAdd.IsEnabled = false;
                Delete.IsEnabled = false;
                ActivateGroupBoxEdit.IsEnabled = false;
                NameTxb.IsEnabled = false;
                ModelTxb.IsEnabled = false;
                ParamsTxb.IsEnabled = false;
                InventoryNumberTxb.IsEnabled = false;
                PriceTxb.IsEnabled = false;
                datapicker.IsEnabled = false;
                Editable.IsEnabled = false;
                var user = db.Security.FirstOrDefault(f => f.id == AuthInfoAbout.Auth);
            }
        }

        private void ResetOut() // сброс фильтрации
        {

            WarehouseGrid.ItemsSource = DB.Connection.Техника.ToList();
            txbSearch.Clear();
        }

        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e) // выход из режима редактирования или добавления
        {
            ViewModel.Mode = ViewMode.View;
        }

        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // меню действия добавить
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Техника();
        }

        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // меню действия редактировать
        {
            ViewModel.Mode = ViewMode.Edit;
            BtnMode.Content = "Редактировать";
            ViewModel.EditableEquipment = new Техника()
            {
                Название = ViewModel.SelectedEquipment.Название,
                Модель = ViewModel.SelectedEquipment.Модель,
                Параметры = ViewModel.SelectedEquipment.Параметры,
                Цена = ViewModel.SelectedEquipment.Цена,
                Дата_последнего_обновления = ViewModel.SelectedEquipment.Дата_последнего_обновления,
                Инвентарный_номер = ViewModel.SelectedEquipment.Инвентарный_номер,
                Код = ViewModel.SelectedEquipment.Код,
            };
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e) // switcher
        {
            if (ViewModel.Mode == ViewMode.Add) // add
            {
                if (!string.IsNullOrEmpty(NameTxb.Text) && !string.IsNullOrEmpty(PriceTxb.Text)
                    && !string.IsNullOrEmpty(ModelTxb.Text) && !string.IsNullOrEmpty(ParamsTxb.Text) && !string.IsNullOrEmpty(InventoryNumberTxb.Text))
                {
                    using (dbMalukovEntities db = new dbMalukovEntities())
                    {
                        var equipment = db.Техника.FirstOrDefault(eq => eq.Инвентарный_номер == ViewModel.EditableEquipment.Инвентарный_номер);
                        if (equipment != null)
                        {
                            MessageBox.Show("Данный инвентарный номер уже используется");
                            return;
                        }
                        db.Техника.Add(ViewModel.EditableEquipment);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Новая техника зарегистрирована на складе!");
                    ViewModel.Equipments.Add(ViewModel.EditableEquipment);
                    ViewModel.Mode = ViewMode.View;
                }
                else
                {
                    MessageBox.Show("Не заполнены поля: Название, Цена, Модель, Параметры, Инвентарный номер", 
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (ViewModel.Mode == ViewMode.Edit) // edit
            {
                using (dbMalukovEntities db = new dbMalukovEntities())
                {
                    var equipment = db.Техника.FirstOrDefault(eq => eq.Код == ViewModel.EditableEquipment.Код);
                    if (equipment == null)
                    {
                        MessageBox.Show("Техника не найдена");
                        return;
                    }
                    if (equipment.Инвентарный_номер != ViewModel.EditableEquipment.Инвентарный_номер)
                    {
                        MessageBox.Show("Инвентарный номер нельзя изменять");
                        return;
                    }
                    equipment.Название = ViewModel.EditableEquipment.Название;
                    equipment.Модель = ViewModel.EditableEquipment.Модель;
                    equipment.Параметры = ViewModel.EditableEquipment.Параметры;
                    equipment.Инвентарный_номер = ViewModel.EditableEquipment.Инвентарный_номер;
                    equipment.Цена = ViewModel.EditableEquipment.Цена;
                    equipment.Дата_последнего_обновления = ViewModel.EditableEquipment.Дата_последнего_обновления;
                    equipment.Код = ViewModel.EditableEquipment.Код;
                    equipment.Выдача = ViewModel.EditableEquipment.Выдача;
                    db.SaveChanges();
                    ViewModel.EditableEquipment = equipment;
                }
                MessageBox.Show("Информация обновлена!");
                for (int i = 0; i < ViewModel.Equipments.Count; i++)
                {
                    if (ViewModel.Equipments[i].Код == ViewModel.EditableEquipment.Код)
                    {
                        ViewModel.Equipments[i] = ViewModel.EditableEquipment;
                    }
                }
                ViewModel.EditableEquipment = null;
                ViewModel.Mode = ViewMode.View;
            }
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск
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
            DataGridToSheet("Склад", WarehouseGrid);
        }
        private void DeleteStringFromGrid_Click(object sender, RoutedEventArgs e) // удаление
        {
            int Id = (WarehouseGrid.SelectedItem as Техника).Код;
            var Indx = (WarehouseGrid.SelectedItem as Техника).Название;
            var Index = (WarehouseGrid.SelectedItem as Техника).Модель;
            var deleteTech = db.Техника.Where(m => m.Код == Id).Single();
            if (MessageBox.Show($"Удалить запись о технике: {Indx} {Index}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                db.Техника.Remove(deleteTech);
                db.SaveChanges();
                WarehouseGrid.ItemsSource = db.Техника.ToList();
                MessageBox.Show("Удаление успешно");
            }
        }

        private void WarehouseGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) // вход в режим редактирования
        {
            if (AuthInfoAbout.Auth == 1)
            {
                ViewModel.Mode = ViewMode.Edit;
                BtnMode.Content = "Редактировать";
                ViewModel.EditableEquipment = new Техника()
                {
                    Название = ViewModel.SelectedEquipment.Название,
                    Модель = ViewModel.SelectedEquipment.Модель,
                    Параметры = ViewModel.SelectedEquipment.Параметры,
                    Цена = ViewModel.SelectedEquipment.Цена,
                    Дата_последнего_обновления = ViewModel.SelectedEquipment.Дата_последнего_обновления,
                    Инвентарный_номер = ViewModel.SelectedEquipment.Инвентарный_номер,
                    Код = ViewModel.SelectedEquipment.Код,
                };
            }
            else { }
        }
    }
}
