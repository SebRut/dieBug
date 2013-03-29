﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

        private bool WindowPickModeEnabled = false;


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
            iiw.Show();
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
            ShootScreen();
        }

        private void ShootScreen()
        {
            this.WindowState = WindowState.Minimized;
            Bitmap screenShot = ScreenCapture.Screen();
            this.WindowState = WindowState.Normal;
            string filename = DateTime.Now.ToShortDateString().Replace(".", "-") + "-" +
                              DateTime.Now.ToShortTimeString().Replace(":", "-") + "-" + DateTime.Now.Second.ToString() + ".png";
            string path = Path.Combine(shotDir, filename);
            screenShot.Save(path, ImageFormat.Png);
            OpenImageInfo(path);
        }

        private void ShootWindowByMouse()
        {
            this.WindowState = WindowState.Minimized;
            Bitmap screenShot = ScreenCapture.Window(ScreenCapture.GetWindowHandleByMouse());
            this.WindowState = WindowState.Normal;
            string filename = DateTime.Now.ToShortDateString().Replace(".", "-") + "-" +
                              DateTime.Now.ToShortTimeString().Replace(":", "-") + "-" + DateTime.Now.Second.ToString() + ".png";
            string path = Path.Combine(shotDir, filename);
            screenShot.Save(path, ImageFormat.Png);
            OpenImageInfo(path);
        }

        private void ShootScreen(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            //ShootScreen();
            ShootWindowByMouse();
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

        private void WindowPickMode()
        {
            while(WindowPickModeEnabled)
            {
                IntPtr windowHandle = ScreenCapture.GetWindowHandleByMouse();
                NativeMethods.SetForegroundWindow(windowHandle);

            }
        }

        private void EnableWindowPickMode(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            WindowPickModeEnabled = true;
            Thread t = new Thread(WindowPickMode);
            t.Start();
        }

        private void DisableWindowPickMode(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            WindowPickModeEnabled = false;
        }
    }
}
