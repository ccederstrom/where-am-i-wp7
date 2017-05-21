using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using Services.Google.Model;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace Services.Google
{
    public class UrlShortener
    {
        public string ShortUrl;

        public event EventHandler ShortUrl_Changed;

        protected virtual void ShortUrlChanged(EventArgs e)
        {
            if (ShortUrl_Changed != null)
                ShortUrl_Changed(this, e);
        }

        public UrlShortener()
        {
            //if (HasNetwork())
            //{
            //    WebClient client = new WebClient();
            //    client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            //    client.Headers["Content-Type"] = "application/json";
            //    client.Encoding = Encoding.UTF8;
            //    client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \"http://www.google.com/\"}");
            //}
        }

        public UrlShortener(string LongURL)
        {
            if (HasNetwork())
            {
                WebClient client = new WebClient();
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                client.Headers["Content-Type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \"" + LongURL + "\"}");
            }
        }


        public void Shorten(string longUrl)
        {
            if (HasNetwork())
            {
                WebClient client = new WebClient();
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                client.Headers["Content-Type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \"" + longUrl + "\"}");
            }
        }

        public UrlShortener(string longURL, TextBox shortUrl)
        {
            //      textBoxShortURL = shortUrl;

            if (HasNetwork())
            {
                WebClient client = new WebClient();
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                client.Headers["Content-Type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \"" + longURL + "\"}");
            }
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            UrlModel deserializedUrlModel = JsonConvert.DeserializeObject<UrlModel>(e.Result);
            //  MessageBox.Show(deserializedUrlModel.id + deserializedUrlModel.kind + deserializedUrlModel.longUrl);
            ShortUrl = deserializedUrlModel.id;
            ShortUrlChanged(EventArgs.Empty);
        }

        private bool HasNetwork()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
                return false;
            }
            return true;
        }
    }

}
