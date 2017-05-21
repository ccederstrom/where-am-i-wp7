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

namespace GPS.Util
{
    public class WebServiceEndPoint_Helper
    {
        private String endpoint {get;set;}
        private int parameternumber;
        public WebServiceEndPoint_Helper(String _endpoint)
        {
            endpoint = _endpoint;
            parameternumber = 0;
        }
        public void AddParameter(String name, String value)
        {
            if (parameternumber == 0)
            {
                endpoint = endpoint + "?" + name + "=" + value;
            }
            else
            {
                endpoint = endpoint + "&"+ name + "=" + value;
            }
            parameternumber++;
        }
        public void AddParameter(String name, Double value)
        {
            AddParameter(name, value.ToString());
        }
        public int getParameterCount()
        {
            return parameternumber;
        }
        public String getEndPoint()
        {
            return endpoint;
        }
    }
}
