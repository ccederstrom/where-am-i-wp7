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
using System.Diagnostics;
using System.Xml.Linq;
using System.Linq;
using Services.Google.Model;
using System.Device.Location;

namespace Services.Google
{
    /// <summary>
    /// Reference: http://www.codeproject.com/Articles/228844/Using-Reverse-Geocoding-to-Find-an-Address
    /// </summary>
    public class ReverseGeocode
    {
        public Address Address;
        public string LongAddress;
        
     //   public string Address;
        public event EventHandler AddressResolved_Complete;

        static string baseUri = "http://maps.googleapis.com/maps/api/" +
                  "geocode/xml?latlng={0},{1}&sensor=false";



        // Invoke the Changed event; called whenever list changes:
        protected virtual void AddressChanged(EventArgs e)
        {
            if (AddressResolved_Complete != null)
                AddressResolved_Complete(this, e);
        }


        public void RetrieveFormatedAddress(string lat, string lng)
        {
            string requestUri = string.Format(baseUri, lat, lng);

            WebClient wc = new WebClient();

            wc.DownloadStringCompleted +=
              new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(new Uri(requestUri));
        }



        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Debug.WriteLine("XML Address: " + e.Result);
            Debug.WriteLine("!!!!!!!!END XML Address");
            try
            {
                var xmlElm = XElement.Parse(e.Result);
                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where elm.Name == "formatted_address"
                               select elm).FirstOrDefault();
                    Debug.WriteLine("formatted_address :::" + res.Value);
                    LongAddress = res.Value;

                    var list = from elm in xmlElm.Descendants()
                               where elm.Name == "long_name"
                               select elm;

                    Address = new Address();
                    if (list.Count() > 0)
                        if (list.ElementAt(0).Value != null)
                        {
                            Debug.WriteLine("street#" + list.ElementAt(0).Value);
                            Address.StreetNumber = list.ElementAt(0).Value;
                        }

                    if (list.Count() > 1)
                        if (list.ElementAt(1).Value != null)
                        {
                            Debug.WriteLine("street name" + list.ElementAt(1).Value);
                            Address.StreetName = list.ElementAt(1).Value;
                        }
                    if (list.Count() > 2)
                        if (list.ElementAt(2).Value != null)
                        {
                            Debug.WriteLine("city" + list.ElementAt(2).Value);
                            Address.City = list.ElementAt(2).Value;
                        }

                    if (list.Count() > 3)
                        if (list.ElementAt(3).Value != null)
                        {
                            Debug.WriteLine("county" + list.ElementAt(3).Value);
                            Address.County = list.ElementAt(3).Value;
                        }

                    if (list.Count() > 5)
                        if (list.ElementAt(5).Value != null)
                        {
                            Debug.WriteLine("state" + list.ElementAt(5).Value);
                            Address.State = list.ElementAt(5).Value;

                        }
                    if (list.Count() > 6)
                        if (list.ElementAt(6).Value != null)
                        {
                            Debug.WriteLine("country" + list.ElementAt(6).Value);
                            Address.Country = list.ElementAt(6).Value;
                        }

                    if (list.Count() > 7)
                        if (list.ElementAt(7).Value != null)
                        {
                            Debug.WriteLine("zip" + list.ElementAt(7).Value);
                            Address.Zip = list.ElementAt(7).Value;
                        }
                    // notify of change
                    AddressChanged(EventArgs.Empty);
                }
                else
                {
                    Debug.WriteLine("No Address Found");
                    //   Address = "No address found";
                }
            }
            catch (Exception x)
            {
         //       MessageBox.Show("Unable to connect to service.");
                Debug.WriteLine("CityGrid download ERROR: " + x.ToString());

            }
        }
    }


    //<GeocodeResponse>
    //  <status>OK</status>
    //  <result>
    //    <type>street_address</type>
    //    <formatted_address>1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA</formatted_address>
    //    <address_component>
    //      <long_name>1600</long_name>
    //      <short_name>1600</short_name>
    //      <type>street_number</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>Amphitheatre Pkwy</long_name>
    //      <short_name>Amphitheatre Pkwy</short_name>
    //      <type>route</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>Mountain View</long_name>
    //      <short_name>Mountain View</short_name>
    //      <type>locality</type>
    //      <type>political</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>San Jose</long_name>
    //      <short_name>San Jose</short_name>
    //      <type>administrative_area_level_3</type>
    //      <type>political</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>Santa Clara</long_name>
    //      <short_name>Santa Clara</short_name>
    //      <type>administrative_area_level_2</type>
    //      <type>political</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>California</long_name>
    //      <short_name>CA</short_name>
    //      <type>administrative_area_level_1</type>
    //      <type>political</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>United States</long_name>
    //      <short_name>US</short_name>
    //      <type>country</type>
    //      <type>political</type>
    //    </address_component>
    //    <address_component>
    //      <long_name>94043</long_name>
    //      <short_name>94043</short_name>
    //      <type>postal_code</type>
    //    </address_component>
    //    <geometry>
    //      <location>
    //        <lat>37.4217550</lat>
    //        <lng>-122.0846330</lng>
    //      </location>
    //      <location_type>ROOFTOP</location_type>
    //      <viewport>
    //        <southwest>
    //          <lat>37.4188514</lat>
    //          <lng>-122.0874526</lng>
    //        </southwest>
    //        <northeast>
    //          <lat>37.4251466</lat>
    //          <lng>-122.0811574</lng>
    //        </northeast>
    //      </viewport>
    //    </geometry>
    //  </result>
    //</GeocodeResponse>
}
