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
using System.IO;
using Path = System.IO.Path;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace dieBug
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            baseDir = Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData)) + Path.DirectorySeparatorChar + "dieBug";
            shotDir = baseDir + Path.DirectorySeparatorChar + "shots";
            InitializeComponent();
            InitFileSystem();
        }
        private string baseDir;
        private string shotDir;

        private void InitFileSystem()
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            if (!Directory.Exists(shotDir))
                Directory.CreateDirectory(shotDir);
        }

        private void OpenImageInfo(string imagePath)
        {
            ImageInfoWindow iiw = new ImageInfoWindow(imagePath);
            this.Visibility = Visibility.Hidden;
            iiw.Show();
            iiw.Closed += iiw_Closed;
        }

        void iiw_Closed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
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
            this.WindowState = WindowState.Minimized;
            Bitmap screenShot = ScreenCapture.Screen();
            this.WindowState = WindowState.Normal;
            string filename = DateTime.Now.ToShortDateString().Replace(".", "-") + "-" + DateTime.Now.ToShortTimeString().Replace(":", "-") + "-" + DateTime.Now.Second.ToString() + ".png";
            string path = Path.Combine(shotDir, filename);
            screenShot.Save(path, ImageFormat.Png);
            OpenImageInfo(path);
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
            Environment.Exit(0);
        }

        private void OpenSettings()
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Show();
        }

        private void OpenAbout()
        {
            AboutWindow aw = new AboutWindow();
            aw.Show();
        }

        private void f1_settings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenSettings();
        }
    }
}
