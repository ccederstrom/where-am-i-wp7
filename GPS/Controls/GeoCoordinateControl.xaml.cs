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
using System.Net.NetworkInformation;
using System.Windows.Threading;
using System.Device.Location;

namespace GPS.Controls
{
    public partial class GeoCoordinateControl : UserControl
    {
        DispatcherTimer infoTimer = new DispatcherTimer();

        public GeoCoordinateControl()
        {
            InitializeComponent();
            App._watcher._watcher.PositionChanged += PositionChanged;
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            App._watcher._watcher.PositionChanged -= PositionChanged;
        }
        /// <summary>
        /// Update the UI with new Geo Coordinate data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (e.Position.Location != null)
                {

                    if (e.Position.Location.Altitude.ToString().Contains("NaN"))
                    {
                        txt_alt.Text = "currently unavailable";
                    }
                    else
                    {
                        txt_alt.Text = Math.Round(e.Position.Location.Altitude,2) + " meters above sea level";
                    }

                    txt_long.Text = e.Position.Location.Longitude.ToString();
                    txt_la.Text = e.Position.Location.Latitude.ToString();

                    if (e.Position.Location.Course.ToString().Contains("NaN"))
                    {
                        txt_course.Text = "currently unavailable";
                    }
                    else
                    {
                        txt_course.Text = e.Position.Location.Course + " degrees north";
                    }
                }
            }
        }


    }
}
