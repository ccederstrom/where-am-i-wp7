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
using System.Windows.Threading;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Globalization;
using ScheduledTaskAgent1.Bing.Geocode;

namespace ScheduledTaskAgent1
{
    public class Address
    {
        public static string CurrentAddress = "";
        public static string CurrentAddress_Locality = "";
        public static string CurrentAddress_PostalCode = "";
        public static string CurrentAddress_AddressLine = "";
        public static string CurrentAddress_AdminDistrict = "";
        public static string CurrentAddress_CountryRegion = "";
        public static void ReverseGeocodeAddress(Dispatcher uiDispatcher, CredentialsProvider credentialsProvider, Location location, Action<GeocodeResult> completed = null, Action<GeocodeError> error = null)
        {
            Debug.WriteLine("inside ReverseGeocodeAddress");
            completed = completed ?? (r => { });
            error = error ?? (e => { });

            // Get credentials and only then place an async call on the geocode service.
            credentialsProvider.GetCredentials(credentials =>
            {
                Debug.WriteLine("inside GetCredentials");
                var request = new ReverseGeocodeRequest()
                {
                    // Pass in credentials for web services call.
                    Credentials = credentials,

                    Culture = CultureInfo.CurrentUICulture.Name,
                    Location = location,

                    // Don't raise exceptions.
                    ExecutionOptions = new ScheduledTaskAgent1.Bing.Geocode.ExecutionOptions
                    {
                        SuppressFaults = true
                    },
                };



                EventHandler<ReverseGeocodeCompletedEventArgs> reverseGeocodeCompleted = (s, e) =>
                {
                    try
                    {
                        if (e.Result.ResponseSummary.StatusCode != ScheduledTaskAgent1.Bing.Geocode.ResponseStatusCode.Success ||
                            e.Result.Results.Count == 0)
                        {
                            // Report geocode error.
                            uiDispatcher.BeginInvoke(() => error(new GeocodeError(e)));
                        }
                        else
                        {
                            // Only report on first result.
                            var firstResult = e.Result.Results[0];
                            uiDispatcher.BeginInvoke(() => completed(firstResult));
                            //Debug.WriteLine("street=" + firstResult.Address.AddressLine.ToString());
                            //Debug.WriteLine("admin district=" + firstResult.Address.AdminDistrict.ToString());
                            //Debug.WriteLine("country region=" + firstResult.Address.CountryRegion.ToString());
                            //Debug.WriteLine("district=" + firstResult.Address.District.ToString());
                            Debug.WriteLine("formatted addres=" + firstResult.Address.FormattedAddress.ToString());
                            //Debug.WriteLine("locality=" + firstResult.Address.Locality.ToString());
                            //Debug.WriteLine("postal code=" + firstResult.Address.PostalCode.ToString());
                            //Debug.WriteLine("postal town=" + firstResult.Address.PostalTown.ToString());
                            CurrentAddress = firstResult.Address.FormattedAddress.ToString();
                            //CurrentAddress_AdminDistrict = firstResult.Address.AdminDistrict.ToString();
                            //CurrentAddress_CountryRegion = firstResult.Address.CountryRegion.ToString();
                            //CurrentAddress_Locality = firstResult.Address.Locality.ToString();
                            //CurrentAddress_PostalCode = firstResult.Address.PostalCode.ToString();
                            //CurrentAddress_AddressLine = firstResult.Address.AddressLine.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        uiDispatcher.BeginInvoke(() => error(new GeocodeError(ex.Message, ex)));
                    }
                };

                var geocodeClient = new GeocodeServiceClient();
                geocodeClient.ReverseGeocodeCompleted += reverseGeocodeCompleted;
                geocodeClient.ReverseGeocodeAsync(request);
            });
        }
    }
}
