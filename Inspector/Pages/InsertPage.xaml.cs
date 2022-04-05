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
    /// Логика взаимодействия для InsertPage.xaml
    /// </summary>
    public partial class InsertPage : Page
    {
        dbMalukovEntities _db = new dbMalukovEntities();
        public InsertPage(Кабинет _кабинет)
        {
            InitializeComponent();

        }

        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            Кабинет newCabinet = new Кабинет()
            {
                Номер_кабинета = Convert.ToInt32(idCabinettextBox.Text),
                Название = cabinettextBox.Text

            };

            _db.Кабинет.Add(newCabinet);
            _db.SaveChanges();
            Handbooks.datagrid.ItemsSource = _db.Кабинет.ToList();
            MessageBox.Show("Кабинет добавлен");
        }
    }
}
