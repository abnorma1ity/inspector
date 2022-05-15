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
using static Inspector.Pages.ExcelHelper;

namespace Inspector.Pages
{
    /// <summary>
    /// Логика взаимодействия для MaterialLiability.xaml
    /// </summary>
    public partial class MaterialLiability : System.Windows.Controls.Page
    {
        public MaterialLiabilityVM ViewModel { get; } = new MaterialLiabilityVM();

        private readonly CollectionViewSource filteremployee;

        public List<Сотрудник> Employeelist;
        public MaterialLiability()
        {
            InitializeComponent();
            if (AuthInfoAbout.Auth != 1) // login
            {
                BtnMode.IsEnabled = false;
                BtnCancel.IsEnabled = false;
                ActivateGroupBoxAdd.IsEnabled = false;
                ActivateGroupBoxEdit.IsEnabled = false;
                var db = new dbMalukovEntities();
                var user = db.Security.FirstOrDefault(f => f.id == AuthInfoAbout.Auth);
            }

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
                Дата_обслуживания = ViewModel.SelectedEquipment.Дата_обслуживания,
                Кабинет = ViewModel.SelectedEquipment.Кабинет,
                Эксплуатация = ViewModel.SelectedEquipment.Эксплуатация,
                ID = ViewModel.SelectedEquipment.ID,
                Сотрудник = ViewModel.SelectedEquipment.Сотрудник,
                Техника = ViewModel.SelectedEquipment.Техника
            };
        }
        private void Export_Click(object sender, RoutedEventArgs e) // export
        {
            DataGridToSheet("Выдача", ResponsobilityGrid);
        }
        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // поиск по названию
        {
            filteremployee?.View.Refresh();
        }
        private void CheckRunning_Click(object sender, RoutedEventArgs e) // клик по чекбоксу Эксплуатация
        {
            filteremployee?.View.Refresh();
        }
        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) // фильтр по сотруднику
        {
            filteremployee?.View.Refresh();
        }

        private void BtnMode_Click(object sender, RoutedEventArgs e) // switcher
        {
            if (ViewModel.Mode == ViewMode.Add) // add
            {
                if (!string.IsNullOrEmpty(Techcmb.Text) || !string.IsNullOrEmpty(Employeecmb.Text))
                {
                    if (!string.IsNullOrEmpty(Techcmb.Text))//&& !string.IsNullOrEmpty(Employeecmb.Text))
                    {
                        if (!string.IsNullOrEmpty(Employeecmb.Text))
                        {
                            var equipment = DB.Выдачи.FirstOrDefault(eq => eq.Код_техники == ViewModel.РедактируемаяВыдача.Код_техники);
                            if (equipment != null)
                            {
                                DB.Выдачи.Remove(equipment);
                            }
                            DB.Connection.Выдача.Add(ViewModel.РедактируемаяВыдача);
                            DB.Connection.SaveChanges();
                            MessageBox.Show("Техника успешно выдана сотруднику!");
                            ViewModel.Выдачи.Add(ViewModel.РедактируемаяВыдача);
                            ViewModel.Mode = ViewMode.View;
                        }
                        else
                        {
                            MessageBox.Show("Не выбран сотрудник для выдачи", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не выбрана техника для выдачи", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Не выбрана техника или сотрудник для выдачи", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (ViewModel.Mode == ViewMode.Edit) // edit
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
                    выдача.Дата_обслуживания = ViewModel.РедактируемаяВыдача.Дата_обслуживания;
                    выдача.Кабинет = ViewModel.РедактируемаяВыдача.Кабинет;
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
            if (ViewModel.ВыбранныйКабинет != DB.ВсеКабинеты && ViewModel.ВыбранныйКабинет != null)
            {
                e.Accepted &= выдача.Кабинет == ViewModel.ВыбранныйКабинет.Номер_кабинета;
            }
            if (ViewModel.ЕслиЭксплуатация)
            {
                e.Accepted &= выдача.Эксплуатация;
            }
            if (!string.IsNullOrEmpty(ViewModel.ПоискТехники))
            {
                if (выдача.Техника == null)
                {
                    e.Accepted = false;

                }
                else
                {
                    e.Accepted &= выдача.Техника.Название.Contains(ViewModel.ПоискТехники) ||
                                  выдача.Техника.Параметры.Contains(ViewModel.ПоискТехники);
                }
            }
        }

        private void cmbCabSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filteremployee?.View.Refresh();
        }

        private void ResponsobilityGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Mode = ViewMode.Edit;
            BtnMode.Content = "Редактировать";
            ViewModel.РедактируемаяВыдача = new Выдача()
            {
                Код_сотр = ViewModel.SelectedEquipment.Код_сотр,
                Код_техники = ViewModel.SelectedEquipment.Код_техники,
                Дата_выдачи = ViewModel.SelectedEquipment.Дата_выдачи,
                Дата_окончания = ViewModel.SelectedEquipment.Дата_окончания,
                Дата_обслуживания = ViewModel.SelectedEquipment.Дата_обслуживания,
                Кабинет = ViewModel.SelectedEquipment.Кабинет,
                Эксплуатация = ViewModel.SelectedEquipment.Эксплуатация,
                ID = ViewModel.SelectedEquipment.ID,
                Сотрудник = ViewModel.SelectedEquipment.Сотрудник,
                Техника = ViewModel.SelectedEquipment.Техника
            };
        }
        private void FilterNonEmpty(object sender, FilterEventArgs e)
        {
            Техника item = (Техника)e.Item;
            e.Accepted = item == null;
        }
        private void WriteoffTech_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.SelectedEquipment == null)
                {
                    return;
                }
                Списание writeoff = new Списание()
                {
                    Дата_списания = System.DateTime.Now,
                    Причина = "11",
                    ФИО_сотр = ViewModel.SelectedEquipment.Сотрудник.ФИО_сотр,
                };
                DB.Connection.Списание.Add(writeoff);
                DB.Connection.SaveChanges();
                var t = ViewModel.SelectedEquipment.Техника;
                //t.Списание = writeoff;
                DB.Connection.Выдача.Remove(ViewModel.SelectedEquipment);
                DB.Connection.SaveChanges();
                t.Код_списания = writeoff.Код;
                DB.Connection.SaveChanges();

                ViewModel.Списания.Add(writeoff);
                for (int i = 0; i < ViewModel.Техники.Count; i++)
                {
                    if (ViewModel.Техники[i] == ViewModel.SelectedEquipment.Техника)
                    {
                        ViewModel.Техники[i] = null;
                        ViewModel.Техники[i] = ViewModel.SelectedEquipment.Техника;
                        break;
                    }
                }
                ViewModel.Выдачи.Remove(ViewModel.SelectedEquipment);
            }
            catch
            {
                MessageBox.Show("Невозможно списать технику", "Внимание",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
