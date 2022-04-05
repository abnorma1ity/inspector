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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var us = db.Security.ToList();
            var result = us.Where(f => f.login == TextLogin.Text && f.password == TextPasw.Password);
            if (result.Any() == true)
            {
                var page = new MainWindow();
                page.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                TextLogin.Focus();
                return;
            }
        }
    }
}
