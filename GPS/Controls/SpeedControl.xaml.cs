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
using System.Windows.Threading;
using System.Net.NetworkInformation;
using System.Device.Location;
using System.Diagnostics;

namespace GPS.Controls
{
    public partial class SpeedControl : UserControl
    {
        DispatcherTimer infoTimer = new DispatcherTimer();
        Double speed;//speed is in meters/second
        public SpeedControl()
        {
            InitializeComponent();
            App._watcher._watcher.PositionChanged += PositionChanged;
            try
            {
                if (App._settings["unit"] == null) App._settings["unit"] = "us";
            }
            catch
            {
                App._settings["unit"] = "us";
            }
        }

        /// <summary>
        /// When GPS position changes requery for new speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (App._watcher != null)
                {
                    Debug.WriteLine("change position !!!");
                    Debug.WriteLine("Speed is !!! " + App._watcher.Speed.ToString());
                    if (App._watcher.Speed.ToString().Contains("NaN"))
                    {
                        txt_speed.Text = "0";
                        speed = 0;
                    }
                    else
                    {
                        speed = App._watcher.Speed;
                        if (((string)App._settings["unit"]).Equals("us"))
                        {
                            txt_speed.Text = App._watcher.SpeedMilesPerHour.ToString();
                            txt_unit.Text = "miles / hour";
                        }
                        else
                        {
                            txt_speed.Text =App._watcher.SpeedKilometersPerHour.ToString();
                            txt_unit.Text = "km / hour";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Change the Speed units to Imperial or Metric
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Tap(object sender, GestureEventArgs e)
        {
            if (App._watcher.Speed.ToString().Contains("NaN"))
            {
                txt_speed.Text = "0";
                speed = 0;
            }
            else
            {
                if (((string)App._settings["unit"]).Equals("us"))
                {
                    App._settings["unit"] = "metric";
                    txt_speed.Text = App._watcher.SpeedKilometersPerHour.ToString();
                    txt_unit.Text = "km / hour";
                }
                else
                {
                    App._settings["unit"] = "us";
                    txt_speed.Text = App._watcher.SpeedMilesPerHour.ToString();
                    txt_unit.Text = "miles / hour";
                }
            }
        }
       
    }
}
