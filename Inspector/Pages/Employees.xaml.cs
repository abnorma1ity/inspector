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
using static Inspector.Pages.ExcelHelper;

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
                Editable.IsEnabled = false;

            }
        }

        private void Export_Click(object sender, RoutedEventArgs e) // экспорт
        {
            DataGridToSheet("Сотрудники", EmployeesGrid);
        }
        private void ResetOut() // сброс фильтра
        {
            EmployeesGrid.ItemsSource = DB.Connection.Сотрудник.ToList();
            txbSearch.Clear();
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск
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
        private void DeactiveGroupBox_Click(object sender, RoutedEventArgs e) // выход из режима редактирования или добавления
        {
            ViewModel.Mode = ViewMode.View;
        }

        private void ActivateGroupBoxAdd_Click(object sender, RoutedEventArgs e) // добавление
        {
            ViewModel.Mode = ViewMode.Add;
            BtnMode.Content = "Добавить";
            ViewModel.EditableEquipment = new Сотрудник();
        }

        private void ActivateGroupBoxEdit_Click(object sender, RoutedEventArgs e) // редактирование
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

        private void DeleteStringFromGrid_Click(object sender, RoutedEventArgs e) // удаление
        {
            int Id = (EmployeesGrid.SelectedItem as Сотрудник).Код_сотр;
            var Indx = (EmployeesGrid.SelectedItem as Сотрудник).ФИО_сотр;
            var db = new dbMalukovEntities();
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись о сотруднике {Indx}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                var deleteEmployee = db.Сотрудник.Where(m => m.Код_сотр == Id).Single();
                db.Сотрудник.Remove(deleteEmployee);
                db.SaveChanges();
                EmployeesGrid.ItemsSource = db.Сотрудник.ToList();
                MessageBox.Show("Удаление успешно");
            }
        }

        private void EmployeesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) // вход в режим редактирования
        {
            if (AuthInfoAbout.Auth == 1)
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
}
