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
using Coding4Fun.Phone.Controls.Data;

namespace GPS
{
    public partial class HelpPage : PhoneApplicationPage
    {
        MarketplaceReviewTask _marketplaceReview = new MarketplaceReviewTask();
        MarketplaceSearchTask _marketplaceSearch = new MarketplaceSearchTask();
        EmailComposeTask _emailComposeTask = new EmailComposeTask();

        public HelpPage()
        {
            InitializeComponent();
            //txtHelp.Text = "We will continue to roll out new features and suggestions.\n\nControls:\nZoom in and out of map with multi-touch.\n\nPrivacy Policy:\nThe location data is retreived from the Bing Map servers to find your current location, geographic coordinates and routes to your destination.";
            txtAppName.Text = PhoneHelper.GetAppAttribute("Title") + " by " + PhoneHelper.GetAppAttribute("Author");
            txtVersion.Text = "Version " + PhoneHelper.GetAppAttribute("Version");
            txtDescription.Text = PhoneHelper.GetAppAttribute("Description");

            if (App.IsTrial == false)
            {
                adcontrol.Visibility = Visibility.Collapsed;
                HelpPagePivot.Height = 780;
            }
        }

        private void btnMarketplace_Click(object sender, RoutedEventArgs e)
        {
            _marketplaceSearch.SearchTerms = "PNGC WP7";
            _marketplaceSearch.Show();
        }

        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            _marketplaceReview.Show();
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            _emailComposeTask.To = "pngc.wp7@hotmail.com";
            _emailComposeTask.Subject = "Where am I? v." + PhoneHelper.GetAppAttribute("Version") +" Feedback";
            _emailComposeTask.Show();
        }

        private void AdControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            adcontrol.Visibility = Visibility.Collapsed;
            HelpPagePivot.Height = 780;
        }
    }
}