using Inspector.Pages;
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
using System.Diagnostics;

using System.Windows.Media.Animation;

namespace Inspector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lnkImgTLG.RequestNavigate += (s, e) => { Process.Start(e.Uri.ToString()); };
            lnkImgOK.RequestNavigate += (s, e) => { Process.Start(e.Uri.ToString()); };
            lnkImgVK.RequestNavigate += (s, e) => { Process.Start(e.Uri.ToString()); };
            lnkToWebSite.RequestNavigate += (s, e) => { Process.Start(e.Uri.ToString()); };
            var db = new dbMalukovEntities();
            var user = db.Security.FirstOrDefault(f => f.id == AuthInfoAbout.Auth);
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WarehouseOpen(object sender, RoutedEventArgs e)
        {
            var page = new Warehouse();
            MainFrame.Navigate(page);
        }

        private void MaterialLiabilityOpen(object sender, RoutedEventArgs e)
        {
            var page = new MaterialLiability();
            MainFrame.Navigate(page);
        }

        private void SpravsClick(object sender, RoutedEventArgs e)
        {
            var page = new Handbooks();
            MainFrame.Navigate(page);
        }

        private void WebSite_MouseEnter(object sender, MouseEventArgs e)
        {
            WebSite.Foreground = Brushes.BlueViolet;
        }

        private void WebSite_MouseLeave(object sender, MouseEventArgs e)
        {
            WebSite.Foreground = Brushes.Black;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e) // telegram enter
        {
            telegram.Width = 55;
            telegram.Height = 55;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 33;
            HeightAnimation.To = 55;
            HeightAnimation.SpeedRatio = 1.5;
            telegram.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void telegram_MouseLeave(object sender, MouseEventArgs e) // telegram leave
        {
            //telegram.Width = 33;
            //telegram.Height = 33;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 55;
            HeightAnimation.To = 33;
            HeightAnimation.SpeedRatio = 1.5;
            telegram.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void ok_MouseEnter(object sender, MouseEventArgs e) // odnoklassniki enter
        {
            ok.Width = 55;
            ok.Height = 55;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 33;
            HeightAnimation.To = 55;
            HeightAnimation.SpeedRatio = 1.5;
            ok.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void ok_MouseLeave(object sender, MouseEventArgs e) // odnoklassniki leave
        {
            ok.Width = 33;
            ok.Height = 33;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 55;
            HeightAnimation.To = 33;
            HeightAnimation.SpeedRatio = 1.5;
            ok.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void vk_MouseEnter(object sender, MouseEventArgs e) // vk enter
        {
            vk.Width = 55;
            vk.Height = 55;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 33;
            HeightAnimation.To = 55;
            HeightAnimation.SpeedRatio = 1.5;
            vk.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void vk_MouseLeave(object sender, MouseEventArgs e) // vk leave
        {
            vk.Width = 33;
            vk.Height = 33;
            DoubleAnimation HeightAnimation = new DoubleAnimation();
            HeightAnimation.From = 55;
            HeightAnimation.To = 33;
            HeightAnimation.SpeedRatio = 1.5;
            vk.BeginAnimation(Label.WidthProperty, HeightAnimation);
        }

        private void EmployeesOpen(object sender, RoutedEventArgs e)
        {
            var page = new Employees();
            MainFrame.Navigate(page);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var db = new dbMalukovEntities();
            var user = db.Security.FirstOrDefault(f => f.id == AuthInfoAbout.Auth);
            if (AuthInfoAbout.Auth != 1)
                AuthInfoAboutqq.Text = $"Вход выполнен как: {user.description}";
            else AuthInfoAboutqq.Text = $"Вход выполнен как: {user.description}";
        }
    }
}
