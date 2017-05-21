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
using System.Diagnostics;
using GPS.Util;
using Services.CityGrid.Model;
using Newtonsoft.Json;

namespace Services.CityGrid
{
    //Reference : CityGrid
    //Link: http://docs.citygridmedia.com/display/citygridv2/Places+API#PlacesAPI-SearchUsingLatitudeandLongitude
    //
    public class CityGrid_Helper
    {
        const Double PUBLISHER = 10000003813;
        #region Variables

        public Respond respond = null;
        public Boolean completed = false;
        String type;
        Double latitude;
        Double longitude;
        Double radius;
        WebServiceEndPoint_Helper webserviceendpoint_helper = new WebServiceEndPoint_Helper("http://api.citygridmedia.com/content/places/v2/search/latlon");
        WebClient client = new WebClient();
        public event EventHandler UploadStringCompleted;
        #endregion

        #region Initialize

        public CityGrid_Helper()
        {
            Debug.WriteLine("CityGrid_Helper");
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers["Content-Type"] = "application/json"; 
            client.Encoding = Encoding.UTF8;
            AddParameters();
        }
        public CityGrid_Helper(String _type,Double _latitude, Double _longitude, Double _radius)
        {
            type = _type;
            latitude = _latitude;
            longitude = _longitude;
            radius = _radius;
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers["Content-Type"] = "application/json";
            client.Encoding = Encoding.UTF8; 
            AddParameters();
        }

        #endregion

        #region Functions

        public void startUploadStringAsync()
        {
            try
            {
                client.UploadStringAsync(new Uri(webserviceendpoint_helper.getEndPoint()), "GET");
            }
            catch (Exception e)
            {
                Debug.WriteLine("startUploadStringAsync ERROR: " + e.ToString());
            }
        }
        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result);
            try
            {
                if (e.Result != null)
                {
                    // Debug.WriteLine("Result is "+ e.Result);
                    respond = JsonConvert.DeserializeObject<Respond>(e.Result);
                    //Debug.WriteLine("URI is " + respond.results.uri);
                    OnUploadStringCompleted();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine("CityGrider_Helper ERROR: " + x.ToString());
            }
        }
        public virtual void OnUploadStringCompleted()
        {
            if (UploadStringCompleted != null) UploadStringCompleted(this, EventArgs.Empty);
        }
        public Boolean isCompleted()
        {
            return completed;
        }
        public Respond getRespond()
        {
            return respond;
        }
        void AddParameters()
        {
            webserviceendpoint_helper.AddParameter("type", type);
            webserviceendpoint_helper.AddParameter("lat", latitude);
            webserviceendpoint_helper.AddParameter("lon", longitude);
            webserviceendpoint_helper.AddParameter("radius", radius);
            webserviceendpoint_helper.AddParameter("publisher", PUBLISHER);
            webserviceendpoint_helper.AddParameter("format", "json");
        }

        #endregion
    }
}
