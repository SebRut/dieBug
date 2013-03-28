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
    }
}
