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
using System.ComponentModel;

namespace UsingBingMaps.Services.CityGrid.Controls.ViewModels
{
    public class LocationViewModel : INotifyPropertyChanged
    {
        private string image;

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (value != image)
                {
                    image = value;
                    NotifyPropertyChanged("Image");
                }
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string sampleCategories;

        public string SampleCategories
        {
            get
            {
                return sampleCategories;
            }
            set
            {
                if (value != sampleCategories)
                {
                    sampleCategories = value;
                    NotifyPropertyChanged("SampleCategories");
                }
            }
        }


        private string phoneNumber;

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    NotifyPropertyChanged("PhoneNumber");
                }
            }
        }

        private string rating;

        public string Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (value != rating)
                {
                    rating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }


        private string website;

        public string Website
        {
            get
            {
                return website;
            }
            set
            {
                if (value != website)
                {
                    website = value;
                    NotifyPropertyChanged("Website");
                }
            }
        }

        private Visibility phoneNumberPanel_Visibility;
        public Visibility PhoneNumberPanel_Visibility
        {
            get
            {
                if (!String.IsNullOrEmpty(PhoneNumber) && !String.IsNullOrWhiteSpace(PhoneNumber)) 
                {
                    return Visibility.Visible;
                }
                else
                    return Visibility.Collapsed;
            }
            set
            {
                if (value != phoneNumberPanel_Visibility)
                {
                    phoneNumberPanel_Visibility = value;
                    NotifyPropertyChanged("PhoneNumberPanel_Visibility");
                }
            }
        }

        private Visibility ratingPanel_Visibility;
        public Visibility RatingPanel_Visibility
        {
            get
            {
                if (!String.IsNullOrEmpty(Rating)&&!String.IsNullOrWhiteSpace(Rating))
                {
                    return Visibility.Visible;
                }
                else
                    return Visibility.Collapsed;
            }
            set
            {
                if (value != ratingPanel_Visibility)
                {
                    ratingPanel_Visibility = value;
                    NotifyPropertyChanged("RatingPanel_Visibility");
                }
            }
        }

        private Visibility websitePanel_Visibility;
        public Visibility WebsitePanel_Visibility
        {
            get
            {
                if (!String.IsNullOrEmpty(Website) && !String.IsNullOrWhiteSpace(Website)) 
                {
                    return Visibility.Visible;
                }
                else
                    return Visibility.Collapsed;
            }
            set
            {
                if (value != websitePanel_Visibility)
                {
                    websitePanel_Visibility = value;
                    NotifyPropertyChanged("WebsitePanel_Visibility");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
