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
using System.Collections.Generic;

namespace Services.CityGrid.Model
{
    public class Location
    {
        public int id;
        public String featured;
        public String public_id;
        public String name;
        public Address address;
        public String neighborhood;
        public Double latitude;
        public Double longitude;
        public Double distance;
        public String image;
        public String phone_number;
        public String fax_number;
        public String rating;
        public String tagline;
        public String profile;
        public String website;
        public Boolean has_video;
        public Boolean has_offers;
        public String offers;
        public String user_reviews_count;
        public String sample_categories;
        public String impression_id;
        public Expension expansion;
        public List<Tag> tags;
    }
}
