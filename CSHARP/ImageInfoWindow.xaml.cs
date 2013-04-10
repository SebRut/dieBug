using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace dieBug
{
    /// <summary>
    ///     Interaktionslogik für ImageInfoWindow.xaml
    /// </summary>
    public partial class ImageInfoWindow
    {
        private readonly string _ImagePath = String.Empty;

        private string _Description = String.Empty;
        private bool _Isbiggered;
        private string _UploadPassword;
        private string _UploadPath;

        public ImageInfoWindow(string imagePath)
        {
            InitializeComponent();
            FileInfo fi = new FileInfo(imagePath);
            this.Title = fi.Name;

            _ImagePath = imagePath;
            try
            {
                ScreenshotImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Die Datei konnte nicht gefunden werden!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            RegistryKey diebugKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\dieBug",
                                                                      RegistryKeyPermissionCheck.ReadSubTree);
            _UploadPath = (string) diebugKey.GetValue("UploadPath", String.Empty);
            _UploadPassword = (string) diebugKey.GetValue("UploadPassword", String.Empty);

            diebugKey.Close();
        }

        private void TransmitPicture()
        {
            var nvc = new Dictionary<string, string>();

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate
                {
                    descriptionbox.IsReadOnly = true;
                    upload.Visibility = Visibility.Hidden;
                    newphoto.Visibility = Visibility.Hidden;
                    delete.Visibility = Visibility.Hidden;
                    progressbarimg.Visibility = Visibility.Visible;
                    progressbarbar.Visibility = Visibility.Visible;
                }));
            nvc.Add("description", _Description);
            string response = HttpUploadFile(_UploadPath, _ImagePath, "datei", "image/png", nvc);
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate
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

        private string HttpUploadFile(string url, string file, string paramName, string contentType,
                                      Dictionary<string, string> nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            var wr = (HttpWebRequest) WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            SetProgressbarToPercentage
                (10);


            int percentperkey = 40/nvc.Keys.Count;
            double value = 10;

            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                SetProgressbarToPercentage(value + percentperkey);
                value += percentperkey;
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            const string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var buffer = new byte[4096];
            int bytesRead = 0;

            int parts = (int) fileStream.Length/4096;
            double percentperpart = 40.0/parts;
            value = 50;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);

                SetProgressbarToPercentage(value + percentperpart);
                value += percentperpart;
            }
            fileStream.Close();

            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            string response = String.Empty;
            try
            {
                wresp = wr.GetResponse();
                SetProgressbarToPercentage
                    (95);
                Stream stream2 = wresp.GetResponseStream();
                if (stream2 != null)
                {
                    var reader2 = new StreamReader(stream2);
                    response = reader2.ReadToEnd();
                    reader2.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    response = "Error";
                }
            }
            finally
            {
                SetProgressbarToPercentage(100);
            }
            return response;
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

            Close();
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

            if (_UploadPath.Equals(String.Empty))
            {
                RegistryKey diebugKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\dieBug",
                                                                          RegistryKeyPermissionCheck.ReadSubTree);
                _UploadPath = (string) diebugKey.GetValue("UploadPath", String.Empty);
                _UploadPassword = (string) diebugKey.GetValue("UploadPassword", String.Empty);

                diebugKey.Close();

                if (MessageBoxResult.Yes ==
                    MessageBox.Show("Es ist kein Uploadpfad eingetragen.\nMöchten sie die Einstellungen öffnen?",
                                    "Es ist ein Fehler aufgetreten.", MessageBoxButton.YesNo))
                {
                    var sw = new SettingsWindow();
                    sw.Show();
                }
            }
            else
            {
                _Description = descriptionbox.Text;
                var t = new Thread(TransmitPicture);
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

        private void SetProgressbarToPercentage(double value)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background,
                                   new Action(delegate { progressbarbar.Width = 384/100*value; }));
        }

        private void ScreenshotImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_Isbiggered)
            {
                ScreenshotImage.Width *= 1.5;
                _Isbiggered = true;
            }
            else
            {
                ScreenshotImage.Width *= 0.75;
                _Isbiggered = false;
            }
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

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            var mw = new MainWindow();
            mw.Show();
        }
    }
}