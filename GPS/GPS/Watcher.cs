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
using System.Device.Location;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Threading;
using Microsoft.Phone.Controls.Maps.Platform;
using UsingBingMaps.Bing.Geocode;
using System.Globalization;
using System.Xml.Linq;
using System.Linq;
using Services.Google;
using Services.Google.Model;

namespace GPS
{
    /// <summary>
    /// Reference: http://msdn.microsoft.com/en-us/library/system.device.location.geocoordinatewatcher.aspx
    /// Bing Map key: AmptuEf6gI3g55lRaJNzSQ47SvHNHaiOy7j2ROkiwOIGyBkrmaDgNQTMot6uOp3O
    /// </summary>
    public class Watcher
    {
        public GeoCoordinateWatcher _watcher;
        public event EventHandler PositionChanged;
        public event EventHandler StatusChanged;

        /// <summary>
        /// Initialize the watcher
        /// </summary>
        public Watcher()
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
        }

        public GeoPositionStatus Status
        {
            get { return _watcher.Status; }
        }

        /// <summary>
        /// Default accuracy
        /// </summary>
        public void StartLocationService()
        {
            StartLocationService(GeoPositionAccuracy.High);
        }

        public void StopLocationService()
        {
            _watcher.Stop();
        }

        public void HighAccuracy()
        {
            StartLocationService(GeoPositionAccuracy.High);
        }

        public void DefaultAccuracy()
        {
            StartLocationService(GeoPositionAccuracy.Default);
        }

        public void StartLocationService(GeoPositionAccuracy accuracy)
        {
            Debug.WriteLine("StartLocationService");
            _watcher.MovementThreshold = 1;
            // Start data acquisition
            _watcher.Start();
        }


        public double Altitude
        {
            get
            {
                return _watcher.Position.Location.Altitude;
            }
        }

        public double Course
        {
            get
            {
                return _watcher.Position.Location.Course;

            }
        }


        public double Latitude
        {
            get
            {
                return _watcher.Position.Location.Latitude;
            }
        }

        public double Longitude
        {
            get
            {
                return _watcher.Position.Location.Longitude;
            }
        }

        public double Speed
        {
            get
            {
                return Math.Round(_watcher.Position.Location.Speed, 2);
            }
        }

        public double SpeedMilesPerHour
        {
            get
            {
                return Math.Round(_watcher.Position.Location.Speed * (3600 / 1609.344), 2);
            }
        }
        public double SpeedKilometersPerHour
        {
            get
            {
                return Math.Round(_watcher.Position.Location.Speed * (3600 / 1000), 2);
            }
        }
        /// <summary>
        /// Indicates that the status of the GeoCoordinateWatcher object has changed. http://msdn.microsoft.com/en-us/library/system.device.location.geocoordinatewatcher.statuschanged.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Debug.WriteLine("watcher_StatusChanged");
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    Debug.WriteLine("disabled");
                    break;
                case GeoPositionStatus.Initializing:
                    Debug.WriteLine("initializing");
                    break;
                case GeoPositionStatus.NoData:
                    Debug.WriteLine("nodata");
                    break;
                case GeoPositionStatus.Ready:
                    Debug.WriteLine("ready");
                    break;
            }
            OnStatusChanged();
        }

        /// <summary>
        /// Indicates that the latitude or longitude of the location data has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Debug.WriteLine(e.Position.Timestamp);
            if (e.Position.Location.IsUnknown)
            {
                Debug.WriteLine("Position unknown");
            }
            else
            {
                PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
            }
            OnPositionChanged();
        }

        /// <summary>
        /// Print the coordinates
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        void PrintPosition(double Latitude, double Longitude)
        {
            Debug.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
        }

        public virtual void OnPositionChanged()
        {
            if (PositionChanged != null) PositionChanged(this, EventArgs.Empty);
        }
        public virtual void OnStatusChanged()
        {
            if (StatusChanged != null) StatusChanged(this, EventArgs.Empty);
        }
    }
}
