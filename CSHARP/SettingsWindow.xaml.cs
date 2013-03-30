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
using Microsoft.Win32;

namespace dieBug
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Minimized;
            LoadValues();
            this.WindowState = WindowState.Normal;
        }

        private void LoadValues()
        {
            RegistryKey diebugKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\dieBug",
                                                                    RegistryKeyPermissionCheck.ReadSubTree);
            UploadUrlPathBox.Text = (string) diebugKey.GetValue("UploadPath", String.Empty);
            PasswordBox.Text = (string) diebugKey.GetValue("UploadPassword", String.Empty);

            diebugKey.Close();

            RegistryKey runKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",
                                                                 true);

            string val = (string) runKey.GetValue("dieBug", "not found");

            if (val == "not found")
                StartupEnabledCBox.IsChecked = false;
            else
                StartupEnabledCBox.IsChecked = true;

            runKey.Close();
        }

        private void SaveSettings()
        {
            RegistryKey diebugKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\dieBug", RegistryKeyPermissionCheck.ReadWriteSubTree);
            diebugKey.SetValue("UploadPath", UploadUrlPathBox.Text);
            diebugKey.SetValue("UploadPassword", PasswordBox.Text);

            diebugKey.Flush();
            diebugKey.Close();

            RegistryKey runKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (StartupEnabledCBox.IsChecked.Value)
            {
                runKey.SetValue("dieBug", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName));
            }
            else
            {
                runKey.DeleteValue("dieBug", false);
            }

            runKey.Flush();
            runKey.Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
