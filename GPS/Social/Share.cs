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
using Microsoft.Phone.Tasks;
using Services.Google;

namespace Social
{
    public class Share
    {
        public  const double INVALID_X_Y =1000;

        public Share()
        {
            
        }

        public void Link(Point location, string address )
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = "My Current Location";
            if (location != null)
            {
                shareLinkTask.LinkUri = new Uri(address, UriKind.Absolute);
            }
            if (address != "")
            {
                shareLinkTask.LinkUri = new Uri(address, UriKind.Absolute);
            }
            shareLinkTask.Message = "";
            shareLinkTask.Show();
        }


        public void Status(Point location, string address, string shortUrl)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            if (address != "")
            {
                shareStatusTask.Status = "My current location: " + address + "\n";
            }
            if (shortUrl != "")
            {
                shareStatusTask.Status += shortUrl + "&form=LMLTCC" + "\n";
            }
            if (location != null)
            {
                if (location.X != INVALID_X_Y && location.Y != INVALID_X_Y)
                    shareStatusTask.Status = shareStatusTask.Status + "Latitude:" + location.X + " Longitude: " + location.Y + "\n";
            }
            shareStatusTask.Show();
        }

        public void Email(Point location, string address, string shortUrl)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.Subject = "GPS Suite";
            //if (StartLocation != null)
            //{
            //    email.Body = "Start Location: X:" + StartLocation.X + " Y: " + StartLocation.Y + "\n";
            //}
            //if (EndLocation != null)
            //{
            //    email.Body = email.Body + "End Location: X:" + EndLocation.X + " Y: " + EndLocation.Y + "\n";
            //}
            if (address != "")
            {
                email.Body = "My current location: " + address + "\n" ;
            }

            if (shortUrl != "")
            {
                email.Body += shortUrl + "\n";
            }
            if (location != null)
            {
                if (location.X != INVALID_X_Y && location.Y != INVALID_X_Y)
                    email.Body = email.Body + "Latitude:" + location.X + " Longitude: " + location.Y + "\n";
            }
            email.Show();
        }


        public void Sms(Point location, string address, string shortUrl)
        {
            SmsComposeTask sms = new SmsComposeTask();
            //sms.To = "0123456789";
            //if (StartLocation != null)
            //{
            //    sms.Body = "Start Location: X:" + StartLocation.X + " Y: " + StartLocation.Y + "\n";
            //}
            //if (EndLocation != null)
            //{
            //    sms.Body = sms.Body + "End Location: X:" + EndLocation.X + " Y: " + EndLocation.Y + "\n";
            //}
            if (address != "")
            {
                sms.Body = "My current location: " + address + "\n" ;
            }
            if (shortUrl != "")
            {
                sms.Body += shortUrl + "\n";
            }
            if (location != null)
            {
                if (location.X != INVALID_X_Y && location.Y != INVALID_X_Y)   
                 sms.Body = sms.Body + "Latitude:" + location.X + " Longitude: " + location.Y + "\n";
            }
            sms.Show();
        }
    }
}
