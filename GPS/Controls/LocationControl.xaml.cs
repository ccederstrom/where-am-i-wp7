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
using GPS;
using System.Device.Location;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using Services.CityGrid.Model;
using Services.CityGrid;
using System.Diagnostics;
using System.Text;
using Microsoft.Phone.Tasks;
using Services.Google;

namespace GPS.Controls
{
    public partial class LocationControl : UserControl
    {
     //   ReverseGeocode _reverseGeocode;
        private CityGrid_Helper _citygrid_helper = new CityGrid_Helper();
        private Location _cityGridLocation = new Location();
        public Location CityGridLocation
        {
            get { return _cityGridLocation; }
            set { _cityGridLocation = value; }
        }

        public LocationControl()
        {
            InitializeComponent();

            App._watcher._watcher.PositionChanged += PositionChanged;
            // App._reverseGeocode = new ReverseGeocode();
            // add listener if Address changes
            App._reverseGeocode.AddressResolved_Complete += UpdateAddress;
            ProgressBar.Visibility = System.Windows.Visibility.Visible;
            // using Location for Add to contacts

            _cityGridLocation.address = new Address();
            //_citygrid_helper = new CityGrid_Helper("", App._watcher.Latitude,App._watcher.Longitude, 1);
            //_citygrid_helper.UploadStringCompleted += new EventHandler(uploadStringCompleted);
            //_citygrid_helper.startUploadStringAsync();
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            App._watcher._watcher.PositionChanged -= PositionChanged;
            App._reverseGeocode.AddressResolved_Complete -= UpdateAddress;
        }

        /// <summary>
        /// Update the Address TextBox with new address
        /// </summary>
        private void UpdateAddress(object sender, EventArgs e)
        {
            ProgressBar.Visibility = System.Windows.Visibility.Visible;
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (App._watcher != null)
                {
                    // Build and address for the location 
                    StringBuilder addressBuilder = new StringBuilder();
                    if (App._reverseGeocode != null)
                    {
                        if (App._reverseGeocode.Address.StreetNumber != null)
                        {
                            addressBuilder.Append(App._reverseGeocode.Address.StreetNumber + " ");
                        }
                        if (App._reverseGeocode.Address.StreetName != null)
                        {
                            addressBuilder.Append(App._reverseGeocode.Address.StreetName + "\n");
                        }
                        if (App._reverseGeocode.Address.City != null)
                        {
                            addressBuilder.Append(App._reverseGeocode.Address.City + ", ");
                        }
                        if (App._reverseGeocode.Address.State != null)
                        {
                            addressBuilder.Append(App._reverseGeocode.Address.State + " ");
                        }
                        if (App._reverseGeocode.Address.Zip != null)
                        {
                            addressBuilder.Append(App._reverseGeocode.Address.Zip);
                        }
                    }
                    txtAddress.Text = addressBuilder.ToString();// CurrentAddress_AddressLine + "\n" + CurrentAddress_Locality + ", " + CurrentAddress_AdminDistrict + " " + CurrentAddress_PostalCode;
                }
                ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// When the GPS position changes requery for new data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            ProgressBar.Visibility = System.Windows.Visibility.Visible;
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (App._watcher != null)
                {
                    //_reverseGeocode.AddressResolved_Complete += UpdateAddress  will listen if the address object changes
                    App._reverseGeocode.RetrieveFormatedAddress(e.Position.Location.Latitude.ToString(), e.Position.Location.Longitude.ToString());
                    // Requery location
                    _citygrid_helper = new CityGrid_Helper("", e.Position.Location.Latitude, e.Position.Location.Longitude, 1);
                    _citygrid_helper.UploadStringCompleted += new EventHandler(uploadStringCompleted);
                    _citygrid_helper.startUploadStringAsync();
                }
                ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void uploadStringCompleted(object sender, EventArgs e)
        {
            Results _result = _citygrid_helper.getRespond().results;
            if (_result.locations.Count != 0)
            {
                Double mindistance = (from location in _result.locations
                                      select location.distance).Min();
                Debug.WriteLine("Shortest Distance is " + mindistance);
                //Need to change to smaller radius
                if (mindistance < 0.01)
                {
                    //stack_currentBus.Visibility = Visibility.Visible;
                    var _nearestlocation = (from location in _result.locations
                                            where location.distance == mindistance
                                            select location).FirstOrDefault();


                    _cityGridLocation.name = _nearestlocation.name;
                    _cityGridLocation.address.address = _nearestlocation.address.address;
                    _cityGridLocation.address.city = _nearestlocation.address.city;
                    _cityGridLocation.address.state = _nearestlocation.address.state;
                    _cityGridLocation.address.postal_code = _nearestlocation.address.postal_code;
                    _cityGridLocation.phone_number = _nearestlocation.phone_number;
                    _cityGridLocation.fax_number = _nearestlocation.fax_number;
                    _cityGridLocation.website = _nearestlocation.website;
                    _cityGridLocation.sample_categories = _nearestlocation.sample_categories;

                    // UI
                    txt_currentbus_name.Text = _nearestlocation.name;
                    txt_currentbus_phonenumber.Text = _nearestlocation.phone_number;
                    txt_currentbus_faxnumber.Text = _nearestlocation.fax_number;
                    txt_currentbus_website.Text = _nearestlocation.website;
                    txt_currentbus_offers.Text = _nearestlocation.offers;

                    if (!_nearestlocation.name.Equals("")) txt_currentbus_name.Visibility = Visibility.Visible;
                    if (!_nearestlocation.phone_number.Equals("")) PhoneStackPanel.Visibility = Visibility.Visible;
                    if (!_nearestlocation.fax_number.Equals("")) FaxStackPanel.Visibility = Visibility.Visible;
                    if (!_nearestlocation.website.Equals("")) WebsiteStackPanel.Visibility = Visibility.Visible;
                    if (!_nearestlocation.offers.Equals("")) OffersStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    txt_currentbus_name.Visibility = Visibility.Collapsed;
                    PhoneStackPanel.Visibility = Visibility.Collapsed;
                    FaxStackPanel.Visibility = Visibility.Collapsed;
                    WebsiteStackPanel.Visibility = Visibility.Collapsed;
                    OffersStackPanel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                txt_currentbus_name.Visibility = Visibility.Collapsed;
                PhoneStackPanel.Visibility = Visibility.Collapsed;
                FaxStackPanel.Visibility = Visibility.Collapsed;
                WebsiteStackPanel.Visibility = Visibility.Collapsed;
                OffersStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Call the business phone #
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_currentbus_phonenumber_Tap(object sender, GestureEventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.DisplayName = txt_currentbus_name.Text;
            phoneCallTask.PhoneNumber = txt_currentbus_phonenumber.Text;
            phoneCallTask.Show();
        }

        /// <summary>
        /// Open browers using the current website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_currentbus_website_Tap(object sender, GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(txt_currentbus_website.Text);
            webBrowserTask.Show();
        }

        /// <summary>
        /// Search for business names with Bing Map Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_currentbus_name_Tap(object sender, GestureEventArgs e)
        {
            BingMapsTask bingMapsTask = new BingMapsTask();
            bingMapsTask.SearchTerm = txt_currentbus_name.Text;
            bingMapsTask.Show();
        }

        /// <summary>
        /// Search the Bing map with current address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_Tap(object sender, GestureEventArgs e)
        {
            BingMapsTask bingMapsTask = new BingMapsTask();
            bingMapsTask.SearchTerm = txtAddress.Text;
            bingMapsTask.Show();
        }


    }
}
