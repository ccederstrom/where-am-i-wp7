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
using Microsoft.Advertising.Mobile.UI;

namespace GPS.PubCenter
{

    enum Mode
    {
        Free = 0,
        Trial = 1,
        BuyOnly = 2
    }

    /// <summary>
    /// Advertising settings for PubCenter ad control. Change the Mode for the try of revenue control.
    /// </summary>
    public class AdSettings
    {
        readonly string _applicationId = "8bb6c0ee-6141-478d-8173-6e0e2abd59e9";
        readonly string _adUnitId = "10031454";

        /// <summary>
        /// Change Mode for the different ad types
        /// </summary>
        Mode _mode = Mode.BuyOnly;

        public AdSettings(ref AdControl adControl, ref Image ad_close)
        {
            // Set the PubCenter identifiers
            adControl = new AdControl(_applicationId,_adUnitId,true);
            adControl.Height = 80;
            adControl.Width = 480;
            #region Free mode
            if (_mode == Mode.Free)
            {
                //    enable ads
                adControl.IsEnabled = true;
                adControl.Visibility = System.Windows.Visibility.Visible;
                adControl.IsAutoCollapseEnabled = true;
                ad_close.Visibility = Visibility.Visible;
            }
            #endregion

            #region Trial mode
            else if (_mode == Mode.Trial)
            {
                if (App.IsTrial == true)
                {
                    //    enable ads
                    adControl.IsEnabled = true;
                    adControl.Visibility = System.Windows.Visibility.Visible;
                    adControl.IsAutoCollapseEnabled = true;
                    ad_close.Visibility = Visibility.Visible;
                }
                else
                {
                    // disables ads
                    adControl.IsAutoCollapseEnabled = true;
                    adControl.Visibility = System.Windows.Visibility.Collapsed;
                    adControl.IsEnabled = false;
                    ad_close.Visibility = Visibility.Collapsed;
                }
            }
            #endregion

            #region BuyOnly mode
            else if (_mode == Mode.BuyOnly)
            {
                // disables ads
                adControl.IsAutoCollapseEnabled = true;
                adControl.IsEnabled = false;
                adControl.Visibility = System.Windows.Visibility.Collapsed;
                ad_close.Visibility = Visibility.Collapsed;
                //adControl.IsAutoRefreshEnabled = false;
            }
            #endregion
        }
    }
}
