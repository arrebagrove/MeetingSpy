using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingSpy.Models
{
    public class Meeting
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<Attendees> Attendees { get; set; }
    }

    public class Attendees
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Url { get; set; }
    }

    public class MeetingViewModel : INotifyPropertyChanged
    {
        public MeetingViewModel()
        {
            this._meeting = null;
        }

        public static MeetingViewModel CreateInstance(Event calendarEvent )
        {
            var instance = new MeetingViewModel();

            instance.Meeting = calendarEvent;

            //instance.Meeting = new Meeting() { Title = "Sync Week", StartTime = "6/6/2016", EndTime = "6/10/2016", Location = "Redmond" };

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

        private Event _meeting;
        public Event Meeting
        {
            get
            {
                return _meeting;
            }

            set
            {
                if (value != this._meeting)
                {
                    this._meeting = value;
                    NotifyPropertyChanged("Meeting");
                }
            }
        }


    }
}
