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
    /// Логика взаимодействия для UpdatePage.xaml
    /// </summary>
    public partial class UpdatePage : Page
    {
        dbMalukovEntities _db = new dbMalukovEntities();
        int Id;

        public UpdatePage(int CabinetId)
        {
            InitializeComponent();
            Id = CabinetId;
        }

        public UpdatePage(Кабинет кабинет)
        {
            Кабинет = кабинет;
        }

        public Кабинет Кабинет { get; }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            Кабинет updateCabinet = (from m in _db.Кабинет
                                     where m.Номер_кабинета == Id
                                     select m).Single();
            updateCabinet.Название = cabinettextBox.Text;
            updateCabinet.Номер_кабинета = Convert.ToInt32(idCabinettextBox.Text);
            _db.SaveChanges();
            Handbooks.datagrid.ItemsSource = _db.Кабинет.ToList();
        }
    }
}
