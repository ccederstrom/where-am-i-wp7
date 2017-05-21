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
using System.Runtime.Serialization;

namespace Services.Google.Model
{
    [DataContract]
    public class UrlModel
    {
        [DataMember]
        public string kind { get; set; }

        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string longUrl { get; set; }
    }
}
