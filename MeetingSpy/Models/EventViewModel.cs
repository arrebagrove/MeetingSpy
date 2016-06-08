using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingSpy.Models
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public EventViewModel()
        {
            this._event = null;
        }

        public static EventViewModel CreateInstance(Event calendarEvent)
        {
            var instance = new EventViewModel();

            instance.Event = calendarEvent;

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

        private Event _event;
        public Event Event
        {
            get
            {
                return _event;
            }

            set
            {
                if (value != this._event)
                {
                    this._event = value;
                    NotifyPropertyChanged("Event");
                }
            }
        }
    }
}
