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
using System.Windows.Shapes;

namespace Inspector
{
    /// <summary>
    /// Логика взаимодействия для WriteoffWindow.xaml
    /// </summary>
    public partial class WriteoffWindow : Window
    {
        private WriteoffWindow()
        {
            InitializeComponent();
        }


        private void OnWriteoffClick(object sender, RoutedEventArgs e) // кнопка ок
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e) // кнопка отмена
        {
            DialogResult = false;
            Close();
        }

        public static (bool? result, string text) Show(Техника техника) // логика для DialogResult
        {
            var wind = new WriteoffWindow
            {
                DataContext = техника
            };
            wind.ShowDialog();
            return (wind.DialogResult, wind.writeoffTxb.Text);
        }
    }
}
