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
    public class Results
    {
        public String rpp;
        public String page;
        public String last_hit;
        public String first_hit;
        public String total_hits;
        public String query_id;
        public String uri;
        public String did_you_mean;
        public List<Region> regions;
        public List<Location> locations;
    }
}
