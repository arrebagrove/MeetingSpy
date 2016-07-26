using System;
using System.Collections.Generic;

namespace MeetingSpy
{
public class Attendee
	{
		public string Name { get; set; }
		public string Email { get; set; }
	}

	public class AppointmentDetails
	{
		public string MeetingTitle { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Duration { get; set; }
		public string LocationName { get; set; }
		public List<Attendee> Attendees { get; set; }

		public AppointmentDetails()
		{
			this.Date = new DateTime();
			this.Duration = TimeSpan.FromHours(1.5);
			this.LocationName = "";
			this.Attendees = new List<Attendee>();
		}
	}
}

