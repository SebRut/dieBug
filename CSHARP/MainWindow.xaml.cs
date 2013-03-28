using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Media.Animation;

namespace dieBug
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            //var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_hover.png", UriKind.Relative);
            //f1_shoot.Source = new BitmapImage(uriSource);
            Storyboard rot1 = (Storyboard)FindResource("f1_shoot_rot1");
            rot1.Begin(this);
        }

        private void f1_shoot_MouseLeave(object sender, MouseEventArgs e)
        {
            //var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_normal.png", UriKind.Relative);
            //f1_shoot.Source = new BitmapImage(uriSource);
            Storyboard rot2 = (Storyboard)FindResource("f1_shoot_rot2");
            rot2.Begin(this);
        }

        private void f1_shoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_active.png", UriKind.Relative);
            f1_shoot.Source = new BitmapImage(uriSource);
        }

        private void f1_shoot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_normal.png", UriKind.Relative);
            f1_shoot.Source = new BitmapImage(uriSource);
        }

        private void f1_background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_normal.png", UriKind.Relative);
            f1_shoot.Source = new BitmapImage(uriSource);
        }

        private void f1_close_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_hover.png", UriKind.Relative);
            f1_close.Source = new BitmapImage(uriSource);
        }

        private void f1_close_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_normal.png", UriKind.Relative);
            f1_close.Source = new BitmapImage(uriSource);
        }

        private void f1_close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_active.png", UriKind.Relative);
            f1_close.Source = new BitmapImage(uriSource);
        }

        private void f1_close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_hover.png", UriKind.Relative);
            f1_close.Source = new BitmapImage(uriSource);
        }
    }
}
