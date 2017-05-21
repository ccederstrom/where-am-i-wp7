using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Advertising.Mobile.UI;
using GPS.PubCenter;
using Services.Google;

namespace GPS
{
    public partial class Share : PhoneApplicationPage
    {


        Point MyLocation;
        string Address = "";
        UrlShortener _urlShortener = new UrlShortener();

        private void ShortUrl_Changed(object sender, EventArgs e)
        {
            btnEmail.IsEnabled = true;
            btnLink.IsEnabled = true;
            btnMessage.IsEnabled = true;
            btnStatus.IsEnabled = true;
        }

        public Share()
        {
            InitializeComponent();
            InitializeAdvertising();
            _urlShortener.ShortUrl_Changed += ShortUrl_Changed;
            // Enable buttons once Short URL has been resolved.. see ShortUrl_Changed
            btnEmail.IsEnabled = false;
            btnLink.IsEnabled = false;
            btnMessage.IsEnabled = false;
            btnStatus.IsEnabled = false;
        }


        private void InitializeAdvertising()
        {
            #region Advertising
            AdControl adControl = new AdControl();
            Image ad_close = new Image();
            AdSettings adSettings = new AdSettings(ref adControl, ref ad_close);
            this.adGrid.Children.Add(adControl);
            #endregion
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Address = (NavigationContext.QueryString["Address"]);
            //StartLocation.X = Double.Parse(NavigationContext.QueryString["StartLocationX"]);
            //StartLocation.Y = Double.Parse(NavigationContext.QueryString["StartLocationY"]);
            //EndLocation.X = Double.Parse(NavigationContext.QueryString["EndLocationX"]);
            //EndLocation.Y = Double.Parse(NavigationContext.QueryString["EndLocationY"]);
            //MyLocation.X = Double.Parse(NavigationContext.QueryString["MyLocationX"]);
            //MyLocation.Y = Double.Parse(NavigationContext.QueryString["MyLocationY"]);
            MyLocation.Y = App._watcher.Longitude;
            MyLocation.X = App._watcher.Latitude;
            _urlShortener.Shorten("http://www.bing.com/maps/?v=2&lvl=16&dir=0&sty=r&where1=" + MyLocation.X + "," + MyLocation.Y + "&form=LMLTCC");
        }
        private void SMS_Click(object sender, EventArgs e)
        {
            Social.Share share = new Social.Share();
            share.Sms(getMyLocation(), Address, getshortURL());
        }

        private void Email_Click(object sender, EventArgs e)
        {
            Social.Share share = new Social.Share();
            share.Email(getMyLocation(), Address, getshortURL());
        }
        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            Social.Share share = new Social.Share();
            share.Status(getMyLocation(), Address, getshortURL());
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            Social.Share share = new Social.Share();
            share.Link(MyLocation, _urlShortener.ShortUrl);
        }
        private Point getMyLocation()
        {
            if (chkbox_geo.IsChecked==true) 
                    return MyLocation;
            else 
                    return new Point(Social.Share.INVALID_X_Y, Social.Share.INVALID_X_Y);
        }
        private String getshortURL()
        {
            if (chkbox_url.IsChecked == true)
                return _urlShortener.ShortUrl;
            else
                return "";
        }
    }
}