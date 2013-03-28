using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            image1.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        }
        
        private void TransmitPicture()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://lol.de/upload.php");
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "multipart/form-data";
            request.KeepAlive = true;
            throw new NotImplementedException();
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
        }


    }
}
