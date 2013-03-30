using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Web;

namespace dieBug
{
    /// <summary>
    /// Interaktionslogik für ImageInfoWindow.xaml
    /// </summary>
    public partial class ImageInfoWindow : Window
    {
        private string imagePath = String.Empty;
        public ImageInfoWindow(string imagePath)
        {
            InitializeComponent();
            this.imagePath = imagePath;
            try
            {
                image1.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Die Datei konnte nicht gefunden werden!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            RegistryKey diebugKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\dieBug", RegistryKeyPermissionCheck.ReadSubTree);
            UploadPath = (string)diebugKey.GetValue("UploadPath", String.Empty);
            UploadPassword = (string)diebugKey.GetValue("UploadPassword", String.Empty);

            diebugKey.Close();
        }

        private string UploadPassword;
        private string UploadPath;
        private string description = String.Empty;
        private bool isbiggered = false;

        private void TransmitPicture()
        {
            Dictionary<string, string> nvc = new Dictionary<string, string>();

            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(delegate()
                {
                    descriptionbox.IsReadOnly = true;
                    upload.Visibility = Visibility.Hidden;
                    newphoto.Visibility = Visibility.Hidden;
                    delete.Visibility = Visibility.Hidden;
                    progressbarimg.Visibility = Visibility.Visible;
                    progressbarbar.Visibility = Visibility.Visible;
                }));
            nvc.Add("description", description);
            string response = HttpUploadFile((string)UploadPath, imagePath, "datei", "image/png", nvc);
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(delegate()
            {
                progressbarimg.Visibility = Visibility.Hidden;
                progressbarbar.Visibility = Visibility.Hidden;
                urlimage.Visibility = Visibility.Visible;
                urlbox.Visibility = Visibility.Visible;
                urlbox.Text = response; 
                urlbox.Focus();
                urlbox.SelectAll();
            }));
        }

        private void textBox_DeliverFrom_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (descriptionbox.LineCount > 5)
                    e.Handled = true;
            }
        }

        private void newphoto_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_new_hover.png", UriKind.Relative);
            newphoto.Source = new BitmapImage(uriSource);
        }

        private void newphoto_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_new_normal.png", UriKind.Relative);
            newphoto.Source = new BitmapImage(uriSource);
        }

        private void newphoto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_new_active.png", UriKind.Relative);
            newphoto.Source = new BitmapImage(uriSource);
        }

        private void newphoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_new_hover.png", UriKind.Relative);
            newphoto.Source = new BitmapImage(uriSource);

            this.Close();
        }

        // --WOOOOOOOH EPIC UPLOAD_BUTTON

        private void upload_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_upload_hover.png", UriKind.Relative);
            upload.Source = new BitmapImage(uriSource);
        }

        private void upload_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_upload_normal.png", UriKind.Relative);
            upload.Source = new BitmapImage(uriSource);
        }

        private void upload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_upload_active.png", UriKind.Relative);
            upload.Source = new BitmapImage(uriSource);
        }

        private void upload_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_upload_hover.png", UriKind.Relative);
            upload.Source = new BitmapImage(uriSource);

            if (UploadPath.Equals(String.Empty))
            {

                RegistryKey diebugKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\dieBug", RegistryKeyPermissionCheck.ReadSubTree);
                UploadPath = (string)diebugKey.GetValue("UploadPath", String.Empty);
                UploadPassword = (string)diebugKey.GetValue("UploadPassword", String.Empty);

                diebugKey.Close();

                if (MessageBoxResult.Yes == MessageBox.Show("Es ist kein Uploadpfad eingetragen.\nMöchten sie die Einstellungen öffnen?", "Es ist ein Fehler aufgetreten.", MessageBoxButton.YesNo))
                {
                    SettingsWindow sw = new SettingsWindow();
                    sw.Show();
                }
            }
            else
            {
                description = descriptionbox.Text;
                Thread t = new Thread(TransmitPicture);
                t.Start();
            }
        }

        // --WOOOOOOOH EPIC DELETE_BUTTON

        private void delete_MouseEnter(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_delete_hover.png", UriKind.Relative);
            delete.Source = new BitmapImage(uriSource);
        }

        private void delete_MouseLeave(object sender, MouseEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_delete_normal.png", UriKind.Relative);
            delete.Source = new BitmapImage(uriSource);
        }

        private void delete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var uriSource = new Uri(@"/dieBug;component/Images/f2_delete_active.png", UriKind.Relative);
            delete.Source = new BitmapImage(uriSource);
        }

        private void delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var uriSource = new Uri(@"/dieBug;component/Images/f2_delete_hover.png", UriKind.Relative);
            delete.Source = new BitmapImage(uriSource);
            Environment.Exit(0);
        }

        public string HttpUploadFile(string url, string file, string paramName, string contentType, Dictionary<string, string> nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate()
            {
                SetProgressbarToPercentage(10);
            }));


            int percentperkey = 40/nvc.Keys.Count;
            double value = 10;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                SetProgressbarToPercentage(value + percentperkey);
                value += percentperkey;
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;

            int parts = (int)fileStream.Length/4096;
            double percentperpart = 40.0 / parts;
            value = 50;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);

                SetProgressbarToPercentage(value + percentperpart);
                value += percentperpart;
            }
            fileStream.Close();
            
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            string response = String.Empty;
            try
            {
                wresp = wr.GetResponse();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate()
                {
                    SetProgressbarToPercentage(95);
                }));
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                response = reader2.ReadToEnd();
                reader2.Dispose();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                    response = "Error";
                }
            }
            finally
            {
                wr = null;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    SetProgressbarToPercentage(100);
                }));
            }
            return response;
        }

        private void SetProgressbarToPercentage(double value)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate()
            {
                progressbarbar.Width = 384 / 100 * value;
            }));
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isbiggered)
            {
                image1.Width *= 1.5;
                isbiggered = true;
            }
            else
            {
                image1.Width *= 0.75;
                isbiggered = false;
            }
        }


        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}