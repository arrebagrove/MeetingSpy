using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MeetingSpy.Models
{
    public class MyContact
    {
        public string DisplayName { get; set; }
        public string Surname { get; set; }
        public EmailAddress[] EmailAddresses { get; set; }

        // derived field
        public string EmailAddress
        {
            get
            {
                return ((EmailAddresses[0] != null) ? EmailAddresses[0].Address : string.Empty);
            }
        }
    }

    public class EmailAddress
    {
        public string Address { get; set; }
        public string Name { get; set; }
    }

    public class ContactViewModel : INotifyPropertyChanged
    {
        public ContactViewModel()
        {
            this._contacts = new ObservableCollection<MyContact>();
        }

        public static ContactViewModel CreateInstance(string content)
        {
            var instance = new ContactViewModel();

            instance.Contacts = new ObservableCollection<MyContact>((JObject.Parse(content)["value"].ToObject<MyContact[]>() as IEnumerable<MyContact>));

            return instance;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private ObservableCollection<MyContact> _contacts;
        public ObservableCollection<MyContact> Contacts
        {
            get
            {
                return _contacts;
            }

            set
            {
                if (value != this._contacts)
                {
                    this._contacts = value;
                    NotifyPropertyChanged("Contacts");
                }
            }
        }


    }
}

