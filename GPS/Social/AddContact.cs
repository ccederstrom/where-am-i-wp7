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
using Microsoft.Phone.UserData;
using Microsoft.Phone.Tasks;
using System.Diagnostics;

namespace Social
{
    public class AddContact
    {
        SaveContactTask _saveContactTask;

        public AddContact(BusinessContact businessContact)
        {
            try
            {
                _saveContactTask = new SaveContactTask();
                _saveContactTask.Company = businessContact.Company;
                _saveContactTask.WorkAddressStreet = businessContact.WorkAddressStreet;
                _saveContactTask.WorkAddressCity = businessContact.WorkAddressCity;
                _saveContactTask.WorkAddressZipCode = businessContact.WorkAddressZipCode;
                _saveContactTask.WorkAddressState = businessContact.WorkAddressState;
                _saveContactTask.WorkAddressCountry = businessContact.WorkAddressCountry;
                _saveContactTask.WorkPhone = businessContact.WorkPhone;
                _saveContactTask.Website = businessContact.Website;
                _saveContactTask.WorkEmail = businessContact.WorkEmail;
                _saveContactTask.Notes = businessContact.Notes;
                _saveContactTask.Show();
            }
            catch (Exception x)
            {
                Debug.WriteLine("AddContact ERROR: " + x.ToString());
                MessageBox.Show("Error occurred adding contact.");
            }
        }

        public AddContact(string company, string workAddressStreet, string workAddressCity, string workAddressState, string workAddressZipCode, string workAddressCountry, string workPhone, string website, string workEmail, string notes)
        {
            try
            {
                _saveContactTask = new SaveContactTask();
                _saveContactTask.Company = company;
                _saveContactTask.WorkAddressStreet = workAddressStreet;
                _saveContactTask.WorkAddressCity = workAddressCity;
                _saveContactTask.WorkAddressZipCode = workAddressZipCode;
                _saveContactTask.WorkAddressState = workAddressState;
                _saveContactTask.WorkAddressCountry = workAddressCountry;
                _saveContactTask.WorkPhone = workPhone;
                _saveContactTask.Website = website;
                _saveContactTask.WorkEmail = workEmail;
                _saveContactTask.Notes = notes;
                _saveContactTask.Show();
            }
            catch (Exception x)
            {
                Debug.WriteLine("AddContact ERROR: " + x.ToString());
                MessageBox.Show("Error occurred adding contact.");
            }
        }
    }
}
