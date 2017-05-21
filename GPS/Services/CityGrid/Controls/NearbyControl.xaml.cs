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
using Services.CityGrid;

using System.Collections.ObjectModel;
using UsingBingMaps.Services.CityGrid.Controls.ViewModels;
using Services.CityGrid.Model;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using Microsoft.Phone.Controls;


namespace UsingBingMaps.Services.CityGrid.Controls
{
    public partial class NearbyControl : UserControl
    {
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<LocationViewModel> Items { get; private set; }

        GPS.Watcher _watcher;
        CityGrid_Helper _cityGrid;
        public NearbyControl(ref GPS.Watcher watcher)
        {
            InitializeComponent();
            _watcher = watcher;
            // show the progress bar
            ProgressBar.Visibility = System.Windows.Visibility.Visible;
            // seach for businesses near current location
            _cityGrid = new CityGrid_Helper("", _watcher.Latitude, _watcher.Longitude, 1);
            _cityGrid.UploadStringCompleted += new EventHandler(uploadStringCompleted);
            _cityGrid.startUploadStringAsync();
            this.Items = new ObservableCollection<LocationViewModel>();
            // bind event handler on watcher position changed
            _watcher._watcher.PositionChanged += PositionChanged;
        }

        /// <summary>
        /// Requery for new data once GPS location changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("NearbyControl Search ");
            _cityGrid = new CityGrid_Helper("", _watcher.Latitude, _watcher.Longitude, 1);
            _cityGrid.UploadStringCompleted += new EventHandler(uploadStringCompleted);
            _cityGrid.startUploadStringAsync();
        }

        /// <summary>
        /// Action for event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadStringCompleted(object sender, EventArgs e)
        {
            // clear the item list
            Items.Clear();
            // show progress bar
            ProgressBar.Visibility = System.Windows.Visibility.Visible;
            // convert city grid location information to view model format
            if (_cityGrid.respond.results.locations != null)
            {
                foreach (Location loc in _cityGrid.respond.results.locations)
                {
                    Debug.WriteLine("Rating:" + loc.rating + "!");
                    this.Items.Add(new LocationViewModel()
                    {
                        Name = loc.name,
                        SampleCategories = loc.sample_categories,
                        Image = loc.image,
                        PhoneNumber = loc.phone_number,
                        Rating = loc.rating,
                        Website = loc.website
                    });
                }
            }
            // Populate a list box with results
            NearbyListBox.ItemsSource = Items;
            // hide progress bar
            ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
        }


        /// <summary>
        /// Search for selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NearbyListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                LocationViewModel selectedItem = (sender as ListBox).SelectedItem as LocationViewModel;
                BingMapsTask bingMapsTask = new BingMapsTask();
                bingMapsTask.SearchTerm = selectedItem.Name;
                bingMapsTask.Show();
            }
            catch (Exception x)
            {
                Debug.WriteLine("NearbyListBox_Tap ERROR: " + x.ToString());
            }
        }

        private void NearbyResultsItemStackPanel_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //Debug.WriteLine("CONTEXT MENU OBJECT" + (sender as StackPanel).Name.ToString());
        }

        private void ContextMenuItem_Call_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CONTEXT MENU OBJECT" + (sender).ToString());
            LocationViewModel context = (sender as MenuItem).DataContext as LocationViewModel;
            Debug.WriteLine("context menu" + context.Name);
            PhoneCallTask pct = new PhoneCallTask();
            pct.DisplayName = context.Name;
            pct.PhoneNumber = context.PhoneNumber;
            pct.Show();

        }

        private void ContextMenuList_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("ContextMenuList_Loaded" + sender.ToString());
            Debug.WriteLine("ContextMenuList_Loaded" + sender.GetType().ToString());
            Debug.WriteLine("ContextMenuList_Loaded" + (sender as ContextMenu).Items[1].ToString());
            Debug.WriteLine(this.DataContext.ToString());
            Debug.WriteLine(((sender as ContextMenu).Items[0] as MenuItem).DataContext);

            try
            {
                LocationViewModel context = (((sender as ContextMenu).Items[0] as MenuItem).DataContext) as LocationViewModel;

                if (context.Website == null || context.Website.Equals(""))
                {
                    // Hide website menu item
                    ((sender as ContextMenu).Items[2] as MenuItem).Visibility = System.Windows.Visibility.Collapsed;
                }

                if (context.PhoneNumber == null || context.PhoneNumber.Equals(""))
                {
                    // hide the call menu item
                    ((sender as ContextMenu).Items[1] as MenuItem).Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine("EXCEPTION ContextMenuList_Loaded: " + x.ToString());
            }
            //Debug.WriteLine("ContextMenuList_Loaded Name: " + context.Name);

            //  LocationViewModel context = (this.DataContext as LocationViewModel);
            // Debug.WriteLine(context.Name.ToString());
            //LocationViewModel context = (sender as StackPanel).DataContext as LocationViewModel;

            //Debug.WriteLine("CONTEXT MENU OBJECT" + context.Name.ToString());
            //if (context.PhoneNumber == null || context.PhoneNumber == "")
            //{

            //}
        }

        private void ContextMenuItem_AddContact_Click(object sender, RoutedEventArgs e)
        {
            LocationViewModel context = (sender as MenuItem).DataContext as LocationViewModel;
            SaveContactTask sct = new SaveContactTask();
            sct.Company = context.Name;
            sct.WorkPhone = context.PhoneNumber;
            sct.Notes = "Category: " + context.SampleCategories;
            if (context.Rating != null && !context.Rating.Equals(""))
            {
                sct.Notes += "\nRating: " + context.Rating;
            }
            sct.Website = context.Website;
            sct.Show();
        }

        private void ContextMenuItem_Website_Click(object sender, RoutedEventArgs e)
        {
            LocationViewModel context = (sender as MenuItem).DataContext as LocationViewModel;
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri(context.Website);
            wbt.Show();
        }

    }
}
