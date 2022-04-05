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
    /// Логика взаимодействия для Handbooks.xaml
    /// </summary>
    public partial class Handbooks : Page
    {
        dbMalukovEntities db = new dbMalukovEntities();
        public static DataGrid datagrid;
        public Handbooks()
        {
            InitializeComponent();
            var db = new dbMalukovEntities();
            CabinetGrid.ItemsSource = db.Кабинет.ToList();
            datagrid = CabinetGrid;
            JobGrid.ItemsSource = db.Должность.ToList();
            DivisionGrid.ItemsSource = db.Подразделение.ToList();
        }

        private void ActivateSearch_Click(object sender, RoutedEventArgs e) // клик по кнопке поиск
        {
            if (firstheader.IsSelected)
            {
                txbSearch.Visibility = Visibility.Visible;
                labelSearchFirst.Visibility = Visibility.Visible;
                txbSearch.Focus();

                txbSearchSecond.Visibility = Visibility.Hidden;
                labelSearchSecond.Visibility = Visibility.Hidden;

                txbSearchThird.Visibility = Visibility.Hidden;
                labelSearchThird.Visibility = Visibility.Hidden;
            }
            if (secondheader.IsSelected)
            {
                txbSearchSecond.Visibility = Visibility.Visible;
                labelSearchSecond.Visibility = Visibility.Visible;
                txbSearchSecond.Focus();

                txbSearch.Visibility = Visibility.Hidden;
                labelSearchFirst.Visibility = Visibility.Hidden;

                txbSearchThird.Visibility = Visibility.Hidden;
                labelSearchThird.Visibility = Visibility.Hidden;
            }
            if (thirdheader.IsSelected)
            {
                txbSearchThird.Visibility = Visibility.Visible;
                labelSearchThird.Visibility = Visibility.Visible;
                txbSearchThird.Focus();

                txbSearchSecond.Visibility = Visibility.Hidden;
                labelSearchSecond.Visibility = Visibility.Hidden;

                txbSearch.Visibility = Visibility.Hidden;
                labelSearchFirst.Visibility = Visibility.Hidden;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ResetOut()
        {
            CabinetGrid.ItemsSource = DB.Connection.Кабинет.ToList();
            JobGrid.ItemsSource = DB.Connection.Должность.ToList();
            DivisionGrid.ItemsSource = DB.Connection.Подразделение.ToList();
            txbSearch.Clear();
            txbSearchSecond.Clear();
            txbSearchThird.Clear();
        }
        private void txbSearchSecond_TextChanged(object sender, TextChangedEventArgs e) // 2
        {
            var db = new dbMalukovEntities();
            var us = db.Должность.ToList();
            IEnumerable<Должность> result = null;
            if (txbSearchSecond.Text.Length > 0)
            {
                result = us.Where(find => find.Должность1.Contains(txbSearchSecond.Text)) ;
                JobGrid.ItemsSource = result.ToList();
            }
            else
            {
                 ResetOut();
            }
        }

        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e) // 1
        {
            var db = new dbMalukovEntities();
            var us = db.Кабинет.ToList();
            IEnumerable<Кабинет> result = null;
            if (txbSearch.Text.Length > 0)
            {
                result = us.Where(find => find.Название.Contains(txbSearch.Text));
                CabinetGrid.ItemsSource = result.ToList();
            }
            else
            {
                ResetOut();
            }
        }

        private void txbSearchThird_TextChanged(object sender, TextChangedEventArgs e) // 3
        {
            var db = new dbMalukovEntities();
            var us = db.Подразделение.ToList();
            IEnumerable<Подразделение> result = null;
            if (txbSearchThird.Text.Length > 0)
            {
                result = us.Where(find => find.Подразделение1.Contains(txbSearchThird.Text));
                DivisionGrid.ItemsSource = result.ToList();
            }
            else
            {
                ResetOut();
            }
        }
        private void insertPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InsertPage(new Кабинет()));
        }
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            int Id = (CabinetGrid.SelectedItem as Кабинет).Номер_кабинета;
            NavigationService.Navigate(new UpdatePage(new Кабинет(Id)));
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int Id = (CabinetGrid.SelectedItem as Кабинет).Номер_кабинета;
            var deleteCabinet = db.Кабинет.Where(m => m.Номер_кабинета == Id).Single();
            db.Кабинет.Remove(deleteCabinet);
            db.SaveChanges();
            CabinetGrid.ItemsSource = db.Кабинет.ToList();
            MessageBox.Show("Удаление успешно");
        }
    }
}
