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

namespace Inspector
{
    /// <summary>
    /// Логика взаимодействия для Writeoff.xaml
    /// </summary>
    public partial class Writeoff : Page
    {
        public Writeoff()
        {
            InitializeComponent();
            var db = new dbMalukovEntities();
            WriteoffGrid.ItemsSource = db.Списание.ToList();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            DataGridToSheet("Списание", WriteoffGrid);
        }
        private void ResetOut()
        {
            WriteoffGrid.ItemsSource = DB.Connection.Списание.ToList();
            txbSearch.Clear();
        }
        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var us = db.Списание.ToList();
            IEnumerable<Списание> result = null;
            if (txbSearch.Text.Length > 0)
            {
                result = us.Where(find => find.ФИО_сотр.Contains(txbSearch.Text));
                WriteoffGrid.ItemsSource = result.ToList();
            }
            else
            {
                ResetOut();
            }
        }
    }
}
