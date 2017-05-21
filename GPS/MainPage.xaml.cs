#define DEBUG_AGENT
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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;

using System.Device.Location;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls.Maps.Platform;
using UsingBingMaps.Bing.Route;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using Microsoft.Phone.Net.NetworkInformation;

using System.Globalization;
using UsingBingMaps.Bing.Geocode;

using GPS.PubCenter;
using Microsoft.Advertising.Mobile.UI;
using Services.CityGrid;
using Services.CityGrid.Model;
using UsingBingMaps.Services.CityGrid.Controls;
using Social;
using GPS.Controls;
using Services.Google;
//using UsingBingMaps.Bing.Route;

namespace GPS
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Variables
        Point SelectedLocation;
        //  GeoCoordinateWatcher watcher;
        Pushpin Current_Location = new Pushpin();
        double longitude;
        double latitude;
        Boolean TrackLocation = true;
        DispatcherTimer mTimer = new System.Windows.Threading.DispatcherTimer();
        #region Getters & Setters

        public string To { get; set; }
        public string From { get; set; }

        #endregion // Getters & Setters
        private readonly CredentialsProvider _credentialsProvider = new ApplicationIdCredentialsProvider(App.Id);
        ProgressIndicator prog;
        bool _isNewPageInstance = false;
        AddressChooserTask addressChooserTask;
        #endregion // Variables

        #region Initialization

        // Constructor

        private void InitializeAdvertising()
        {
            #region Advertising
            AdControl adControl = new AdControl();
            AdSettings adSettings = new AdSettings(ref adControl, ref ad_close);
            adSpace.Children.Clear();
            this.adSpace.Children.Add(adControl);
            #endregion
        }


        private void StartWatcher()
        {
            App._watcher.StartLocationService(GeoPositionAccuracy.High);
            txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Collapsed;
            TrackLocation = true;
        }

        private void StopWatcher()
        {
            App._watcher.StopLocationService();
            txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Visible;
            TrackLocation = false;
        }


        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine("MainPage");
            Map.ZoomLevel = 9;
            InitializeAdvertising();
            App.Current.Host.Settings.MaxFrameRate = 30;
            //Application.Current.Host.Settings.EnableFrameRateCounter = true;
            Debug.WriteLine("Acceleration: " + App.Current.Host.Settings.EnableGPUAcceleration);

            #region Progress Indicator

            InitializeProgressIndicator();

            #endregion

            // enable app bar buttons
            App._reverseGeocode.AddressResolved_Complete += GoogleAddressResolved;

            _isNewPageInstance = true; // tombstoning
            Map.ScaleVisibility = System.Windows.Visibility.Visible;
            Debug.WriteLine("Map height: " + Map.ActualHeightProperty);
            Debug.WriteLine("Map width: " + Map.ActualHeightProperty);
            Debug.WriteLine("Map cache mode: " + Map.CacheMode);
            // caching
            Current_Location.CacheMode = new BitmapCache();
            // Data context and observable collection are children of the main page.
            this.DataContext = this;
            updateMyLocationPushPin(Current_Location.Location);
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Cannot connect to server.");
            }
           
            //Ads Timer
            mTimer.Interval = new TimeSpan(0, 0, 0, 0, 300000); // 5 mins
            mTimer.Tick += new EventHandler(mTimer_Tick);


        }

        /// <summary>
        /// Fancy UI to show progress of downloading map
        /// </summary>
        private void InitializeProgressIndicator()
        {
            SystemTray.SetIsVisible(this, true);
            SystemTray.SetOpacity(this, 0.5);
            SystemTray.SetBackgroundColor(this, Colors.Black);
            SystemTray.SetForegroundColor(this, Colors.White);
            prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = true;
            SystemTray.SetProgressIndicator(this, prog);
        }

        #endregion // Intitialization

        #region Events Handlers
        private void Contact_Click(object sender, EventArgs e)
        {
            addressChooserTask = new AddressChooserTask();
            addressChooserTask.Completed += new EventHandler<AddressResult>(ContactSearch_Completed);
            addressChooserTask.Show();
        }

        //Main Function: Update the info panel
        private void PositionChanged(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (TrackLocation == true && App._watcher != null)
                {
                    // Check if tracker is on and center.
                    if (TrackLocation == true)
                        Map.Center = new GeoCoordinate(App._watcher.Latitude, App._watcher.Longitude);
                    //Map.ZoomBarVisibility = System.Windows.Visibility.Visible;
                    //---set the location for the Current_Location pushpin---
                    Current_Location.Location = new GeoCoordinate(App._watcher.Latitude, App._watcher.Longitude);
                }
            }
        }

        private void StatusChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Status changed : "+App._watcher.Status);
            if (App._watcher.Status == GeoPositionStatus.Disabled)
            {
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = false;
                txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = true;
                txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Map_MouseLeftButtonDown");
            SelectedLocation = e.GetPosition(this.Map);
        }


        private void Share_Click(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (App._watcher != null)
                    if (App._watcher.Status == GeoPositionStatus.Disabled)
                    {
                        MessageBox.Show("Please Turn Location Service On");
                    }
                    else
                    {
                        Debug.WriteLine("Share_Click");
                        NavigationService.Navigate(new Uri("/Share.xaml?Address=" + App._reverseGeocode.LongAddress + "&MyLocationX=" + App._watcher.Latitude + "&MyLocationY=" + App._watcher.Longitude
                        , UriKind.RelativeOrAbsolute));
                    }
            }
            else
            {
                MessageBox.Show("Cannot Connect To Server");
            }
        }
        private void Map_MapResolved(object sender, EventArgs e)
        {
            Debug.WriteLine("Map_MapResolved");
            if (Map.IsDownloading)
            {
                prog.IsVisible = true;
                SystemTray.SetIsVisible(this, true);
            }
            else
            {
                prog.IsVisible = false;
                SystemTray.SetIsVisible(this, false);
            }
        }


        private void Help_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (TrackLocation == false)
            {
                Debug.WriteLine("STRAAAAAAAAAAAAAAT");
                StartWatcher();
                //       MessageBox.Show("Location Service is On");
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = "Stop";

            }
            else
            {
                Debug.WriteLine("STOOOOOOOOOOOP");
                if (App._watcher != null)
                {
                    StopWatcher();
                    //  MessageBox.Show("Location Service is Off");
                    ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = "Start";
                }
            }
        }


        private void BingMapButton_Click(object sender, RoutedEventArgs e)
        {
            BingMapsTask bingMapsTask = new BingMapsTask();
            bingMapsTask.ZoomLevel = 16;
            bingMapsTask.Center = Map.Center;
            bingMapsTask.Show();
        }

        private void NearBySearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BingMapsTask bingMapsTask = new BingMapsTask();
                //  bingMapsTask.ZoomLevel = 16;
                bingMapsTask.Center = Map.Center;
                bingMapsTask.SearchTerm = NearBySearchTextBox.Text;
                bingMapsTask.Show();
                e.Handled = true;
                //   MessageBox.Show("You pressed Enter!");
            }
        }

        void ContactSearch_Completed(object sender, AddressResult e)
        {
            try
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
                    LabeledMapLocation endLabeledMapLocation = new LabeledMapLocation(e.Address, null);
                    bingMapsDirectionsTask.End = endLabeledMapLocation;
                    bingMapsDirectionsTask.Show();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine("Contact Search Error: " + x.ToString());
                MessageBox.Show("Malformed address");
            }
        }

        private void AdControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            adcontrol.Visibility = Visibility.Collapsed;
            mTimer.Start();
        }

        private void NearBySearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.ApplicationBar.IsVisible = !this.ApplicationBar.IsVisible;
            NearbyCityGridResults.Visibility = System.Windows.Visibility.Collapsed;
            NearByScroll.IsHitTestVisible = false;
            MainPivot.IsHitTestVisible = false;
        }

        private void NearBySearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.ApplicationBar.IsVisible = !this.ApplicationBar.IsVisible;
            NearByScroll.IsHitTestVisible = true;
            MainPivot.IsHitTestVisible = true;
            NearbyCityGridResults.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion //Events Handlers

        #region Application Logic

        /// <summary>
        /// Show the Advertisement on tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mTimer_Tick(object sender, EventArgs e)
        {
            adcontrol.Visibility = Visibility.Visible;
        }

        //        public EventHandler<AddressResult> addressChooserTask_Completed { get; set; }
        private void updateMyLocationPushPin(GeoCoordinate loc)
        {
            Grid ret = new Grid();
            ret.Height = 29.284;
            ret.Width = 29.284;
            ret.Visibility = System.Windows.Visibility.Visible;

            // Colors
            SolidColorBrush yellow = new SolidColorBrush();
            yellow.Color = Color.FromArgb(255, 255, 255, 0);
            SolidColorBrush black = new SolidColorBrush();
            black.Color = Color.FromArgb(255, 0, 0, 0);
            SolidColorBrush white = new SolidColorBrush();
            white.Color = Color.FromArgb(255, 255, 255, 255);

            // Rectangle
            Point rectPoint = new Point(0.5, 0.5);
            CompositeTransform rectRotate = new CompositeTransform();
            rectRotate.Rotation = 45;
            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            rect.Height = 20;
            rect.Width = 20;
            rect.StrokeLineJoin = PenLineJoin.Miter;
            rect.RenderTransformOrigin = rectPoint;
            rect.Fill = black;
            rect.Stroke = white;
            rect.UseLayoutRounding = true;
            rect.RenderTransform = rectRotate;

            // Circle
            Ellipse circ = new Ellipse();
            circ.Width = 10;
            circ.Height = 10;
            circ.Stretch = Stretch.Fill;
            circ.StrokeLineJoin = PenLineJoin.Miter;
            circ.Stroke = yellow;
            circ.Fill = yellow;

            // Add elements to the grid
            ret.Children.Add(rect);
            ret.Children.Add(circ);
            // Remove current location icon

            //current_position_layer.Children.Remove(curLoc);
            //curLoc = ret;

            //current_position_layer.AddChild(ret, loc);
            if (Map.Children.Contains(ret) != true)
            {
                Map.Children.Add(ret);
            }
            else
            {
                Map.Children.Remove(ret);
                Map.Children.Add(ret);
            }
        }
        #endregion // Application Logic


        #region Navigation

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("OnNavigatedTo");
            #region RestorePageState

            // Zoom in on map
            //MapZoomIn.Begin();
            base.OnNavigatedTo(e);
            _isNewPageInstance = false;
            #endregion
            try
            {
                // Get the settings for this application.
                App._settings = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + err.ToString());
            }

            #region Settings

            if (App._settings.Contains("MapCenterLocation"))
            {
                Map.Center = (GeoCoordinate)App._settings["MapCenterLocation"];
            }
            else
            {
                // need current location
            }


            if (App._settings.Contains("AerialView"))
            {
                bool view = (bool)App._settings["AerialView"];
                if (view == true)
                {

                    Map.Mode = new AerialMode();
                }
                else
                {
                    Map.Mode = new RoadMode();
                }
            }
            else
            {
                Map.Mode = new RoadMode();
            }
            if (App._settings.Contains("TrackLocation"))
            {
                Debug.WriteLine("TrackLocation found in IsolatedStorage");
                if (App._settings["TrackLocation"].Equals(true))
                {
                    Debug.WriteLine("TrackLocation = true in isolated storage");
                    TrackLocation = true;
                }
                else
                {
                    Debug.WriteLine("TrackLocation = false in isolated storage");
                    TrackLocation = false;
                }
            }
            else
            {
                Debug.WriteLine("TrackLocation NOT FOOUND  setting to true");
                TrackLocation = true;
            }
            #endregion



            if (TrackLocation == true)
            {
                StartWatcher();
            }
            App._watcher.PositionChanged += new EventHandler(PositionChanged);
            App._watcher.StatusChanged += new EventHandler(StatusChanged);
            // update
            if (TrackLocation == false)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = "Start";
            }
            else
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = "Stop";
            }
            Debug.WriteLine("onNavaigateTo GPS Status : " + App._watcher.Status);
            if (App._watcher.Status == GeoPositionStatus.Disabled)
            {
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = false;
                txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = true;
                txtbLocationServiceStatus.Visibility = System.Windows.Visibility.Collapsed;
            }

            #region User Controls
            NearbyCityGridResults.Children.Clear();
            NearbyCityGridResults.Children.Add(new NearbyControl(ref App._watcher));

            LocationControlStackPanel.Children.Clear();
            LocationControlStackPanel.Children.Add(new LocationControl());

            SpeedControlStackPanel.Children.Clear();
            SpeedControlStackPanel.Children.Add(new SpeedControl());

            GeoCoordinateStackPanel.Children.Clear();
            GeoCoordinateStackPanel.Children.Add(new GeoCoordinateControl());

            #endregion

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("OnNavigatedFrom");
            #region SaveState
            //State["SelectedLocation"] = SelectedLocation;
            //if (App._watcher != null)
            //    State["watcher"] = App._watcher;
            //State["Current_Location"] = Current_Location;
            State["longitude"] = longitude;
            State["latitude"] = latitude;


            if (NearBySearchTextBox != null)
                State["NearBySearch"] = NearBySearchTextBox.Text;

            //State["Map"] = Map;
            // assign saved stats to local variables
            App._settings["MapCenterLocation"] = Map.Center;
            App._settings["TrackLocation"] = TrackLocation;
            App._settings.Save();

            #endregion

            if (App._watcher != null)
            {
                if (App._watcher.Status == GeoPositionStatus.Ready)
                {
                    StopWatcher();
                }

                App._watcher.PositionChanged -= new EventHandler(PositionChanged);
                App._watcher.StatusChanged -= new EventHandler(StatusChanged);
            }
            // Call the base method.
            base.OnNavigatedFrom(e);
        }

        #endregion // Navigation




        /// <summary>
        /// Enable Share and Add Contact app bar buttons once address is resolved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoogleAddressResolved(object sender, EventArgs e)
        {
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = true;
        }

        private void AddContact_Click(object sender, EventArgs e)
        {
            // object added to contact
            BusinessContact businessContact = new BusinessContact();

            if (App._reverseGeocode.Address.StreetNumber != null)
            {
                businessContact.WorkAddressStreet = App._reverseGeocode.Address.StreetNumber;
            }
            if (App._reverseGeocode.Address.StreetName != null)
            {
                businessContact.WorkAddressStreet += " " + App._reverseGeocode.Address.StreetName;
            }
            if (App._reverseGeocode.Address.City != null)
            {
                businessContact.WorkAddressCity = App._reverseGeocode.Address.City;
            }
            if (App._reverseGeocode.Address.State != null)
            {
                businessContact.WorkAddressState = App._reverseGeocode.Address.State;
            }
            if (App._reverseGeocode.Address.Zip != null)
            {
                businessContact.WorkAddressZipCode = App._reverseGeocode.Address.Zip;
            }


            foreach (UIElement ui in LocationControlStackPanel.Children)
            {
                if (ui.GetType() == typeof(LocationControl))
                {
                    LocationControl cityGridInfo = ui as LocationControl;
                    try
                    {
                        if (cityGridInfo.CityGridLocation.name != null)
                            businessContact.Company = cityGridInfo.CityGridLocation.name;
                        if (cityGridInfo.CityGridLocation.address.address != null)
                            businessContact.WorkAddressStreet = cityGridInfo.CityGridLocation.address.address;
                        if (cityGridInfo.CityGridLocation.address.city != null)
                            businessContact.WorkAddressCity = cityGridInfo.CityGridLocation.address.city;
                        if (cityGridInfo.CityGridLocation.address.state != null)
                            businessContact.WorkAddressState = cityGridInfo.CityGridLocation.address.state;
                        if (cityGridInfo.CityGridLocation.address.postal_code != null)
                            businessContact.WorkAddressZipCode = cityGridInfo.CityGridLocation.address.postal_code;

                        businessContact.WorkAddressCountry = "country";
                        if (cityGridInfo.CityGridLocation.phone_number != null)
                            businessContact.WorkPhone = cityGridInfo.CityGridLocation.phone_number;
                        if (cityGridInfo.CityGridLocation.website != null)
                            businessContact.Website = cityGridInfo.CityGridLocation.website;
                        businessContact.WorkEmail = "workEmail";
                        if (cityGridInfo.CityGridLocation.sample_categories != null)
                            businessContact.Notes = cityGridInfo.CityGridLocation.sample_categories;
                    }
                    catch (Exception x)
                    {
                        Debug.WriteLine("AddContact_Click ERROR: " + x.ToString());
                        MessageBox.Show("Error adding contact");
                    }
                }
            }
            AddContact addContact = new AddContact(businessContact);
        }


        /// <summary>
        /// Change the application bar depending on the pivot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 2:
                    ApplicationBar.Mode = ApplicationBarMode.Minimized; 
                    break;
                default:
                    ApplicationBar.Mode = ApplicationBarMode.Default;
                    break;
            }
        }


    } // Class
} // Namespace
