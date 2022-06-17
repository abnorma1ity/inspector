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

namespace Inspector.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddMaterialLiabilityPage.xaml
    /// </summary>
    /// тест
    public partial class AddMaterialLiabilityPage : Page
    {
        dbMalukovEntities tech;
        public AddMaterialLiabilityPage(Выдача _responsobility)
        {
            bindcombo();

            InitializeComponent();
            MyResponsobility = _responsobility;
            this.DataContext = this;

            tech = new dbMalukovEntities();
            combotype.ItemsSource = tech.Техника.OrderBy(x => x.Название).ToList();
            combotype.DisplayMemberPath = "Название";
            combotype.SelectedValuePath = "Код"; // -1 ?

            Sotr.ItemsSource = tech.Сотрудник.OrderBy(x => x.ФИО_сотр).ToList();
            Sotr.DisplayMemberPath = "ФИО_сотр";
            Sotr.SelectedValuePath = "Код_сотр";
        }
        public List<Техника> Tech { get; set; }
        public Выдача MyResponsobility { get; set; }
        private void bindcombo()
        {
            dbMalukovEntities dc = new dbMalukovEntities();
            var item = dc.Техника.ToList();
            Tech = item;
            DataContext = Tech;
        }
        private void combotype_SelectionChanged(object sender, SelectionChangedEventArgs e) // выбор техники
        {
            int k = (combotype.SelectedItem as Техника).Код;
            MessageBox.Show(k.ToString());
            var us = tech.Техника.ToList();

            var result = us.Where(f => f.Код == k).First();

            info.Text = result.Параметры.ToString();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) // первый пикер
        {
           CheckRunning.IsChecked = true;
        }

        private void date2_SelectedDateChanged(object sender, SelectionChangedEventArgs e) // второй пикер
        {
            CheckRunning.IsChecked = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e) // кнопка сохранить
        {
            MyResponsobility.Код_техники = (combotype.SelectedItem as Техника).Код;
            MessageBox.Show(MyResponsobility.Код_техники.ToString());
            MyResponsobility.Код_сотр = (Sotr.SelectedItem as Сотрудник).Код_сотр;
            MyResponsobility.Дата_выдачи = date1.SelectedDate;
            MyResponsobility.Дата_окончания = date2.SelectedDate;
            MyResponsobility.Эксплуатация = (bool)CheckRunning.IsChecked;

            DB.Connection.Выдача.Add(MyResponsobility);
            DB.Connection.SaveChanges();
            MessageBox.Show("Техника выдана");
        }
    }
}
