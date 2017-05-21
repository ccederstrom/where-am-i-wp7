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

namespace Social
{
    public class BusinessContact
    {
        public string Company;
        public string WorkAddressStreet;
        public string WorkAddressCity;
        public string WorkAddressZipCode;
        public string WorkAddressState;
        public string WorkAddressCountry;
        public string WorkPhone;
        public string Website;
        public string WorkEmail;
        public string Notes;

        public BusinessContact()
        {
        }

        public BusinessContact(string company, string workAddressStreet, string workAddressCity, string workAddressState, string workAddressZipCode, string workAddressCountry, string workPhone, string website, string workEmail, string notes)
        {
            Company = company;
            WorkAddressStreet = workAddressStreet;
            WorkAddressCity = workAddressCity;
            WorkAddressZipCode = workAddressZipCode;
            WorkAddressState = workAddressState;
            WorkAddressCountry = workAddressCountry;
            WorkPhone = workPhone;
            Website = website;
            WorkEmail = workEmail;
            Notes = notes;
        }
    }
}
