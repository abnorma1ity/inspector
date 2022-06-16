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
        public int count = 0;
        public int failCount = 3;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) // кнопка закрыть
        {
            this.Close();
        }

        private void btnInput_Click(object sender, RoutedEventArgs e) // действия на кнопку Войти
        {
            string strForm2Text = TextLogin.Text;
            string strPassword = TextPasw.Password;
            var db = new dbMalukovEntities();
            var us = db.Security.ToList();
            var result = us.Where(f => f.login == strForm2Text && f.password == strPassword);
            if (result.Any() == true) // администратор
            {
                if (TextLogin.Text == "admin")
                {
                    AuthInfoAbout.Auth = 1;
                }
                else
                {
                    AuthInfoAbout.Auth = 2;
                }
                count = 0;
                MainWindow page = new MainWindow();
                page.Show();
                this.Close();
            }
            else
            {
                failCount--;
                count++;
                MessageBox.Show($"Неверный логин или пароль. Осталось попыток входа: {failCount}");
                TextLogin.Focus();
                if (count == 3)
                {
                    MessageBox.Show($"Введены неверный логин или пароль в кол-ве {count} раз, приложение будет закрыто!");
                    this.Close();
                    return;
                }
            }
        }
    }
}
