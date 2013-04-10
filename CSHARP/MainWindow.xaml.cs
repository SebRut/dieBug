using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace dieBug
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _BaseDir;
        private readonly string _ShotDir;

        public MainWindow()
        {
            _BaseDir = Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData)) +
                       Path.DirectorySeparatorChar + "dieBug";
            _ShotDir = _BaseDir + Path.DirectorySeparatorChar + "shots";
            InitializeComponent();
            InitFileSystem();
        }

        private void InitFileSystem()
        {
            if (!Directory.Exists(_BaseDir))
                Directory.CreateDirectory(_BaseDir);
            if (!Directory.Exists(_ShotDir))
                Directory.CreateDirectory(_ShotDir);
        }

        private void OpenAbout()
        {
            var aw = new AboutWindow();
            aw.Show();
        }

        private void OpenImageInfo(string imagePath)
        {
            var iiw = new ImageInfoWindow(imagePath);
            Visibility = Visibility.Hidden;
            iiw.Show();
        }

        private void OpenSettings()
        {
            var sw = new SettingsWindow();
            sw.Show();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_normal.png", UriKind.Relative);
            F1Shoot.Source = new BitmapImage(uriSource);
        }

        private void F1Shoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_active.png", UriKind.Relative);
            F1Shoot.Source = new BitmapImage(uriSource);
        }

        private void F1Shoot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f1_button_shoot_normal.png", UriKind.Relative);
            F1Shoot.Source = new BitmapImage(uriSource);
            WindowState = WindowState.Minimized;
            Bitmap screenShot = ScreenCapture.Screen();
            WindowState = WindowState.Normal;
            string filename = DateTime.Now.ToShortDateString().Replace(".", "-") + "-" +
                              DateTime.Now.ToShortTimeString().Replace(":", "-") + "-" +
                              DateTime.Now.Second.ToString(CultureInfo.InvariantCulture) + ".png";
            string path = Path.Combine(_ShotDir, filename);
            screenShot.Save(path, ImageFormat.Png);
            OpenImageInfo(path);
            Close();
        }

        private void f1_background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void F1Close_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_hover.png", UriKind.Relative);
            F1Close.Source = new BitmapImage(uriSource);
        }

        private void F1Close_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_normal.png", UriKind.Relative);
            F1Close.Source = new BitmapImage(uriSource);
        }

        private void F1Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_active.png", UriKind.Relative);
            F1Close.Source = new BitmapImage(uriSource);
        }

        private void F1Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_close_hover.png", UriKind.Relative);
            F1Close.Source = new BitmapImage(uriSource);
            Environment.Exit(0);
        }

        private void F1Settings_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_options_hover.png", UriKind.Relative);
            F1Settings.Source = new BitmapImage(uriSource);
        }

        private void F1Settings_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_options_normal.png", UriKind.Relative);
            F1Settings.Source = new BitmapImage(uriSource);
        }

        private void F1Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_options_active.png", UriKind.Relative);
            F1Settings.Source = new BitmapImage(uriSource);
        }

        private void F1Settings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/fall_button_options_hover.png", UriKind.Relative);
            F1Settings.Source = new BitmapImage(uriSource);
            OpenSettings();
        }
    }
}