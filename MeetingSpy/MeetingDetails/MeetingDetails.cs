using System;
using Xamarin.Forms;

namespace MeetingSpy
{
	public class MeetingDetails : ContentPage
	{
		private StackLayout _layoutRoot;

		private bool _firstVisit = true;

		private MeetingDetailsUI _controller;


		public MeetingDetails()
		{
			this.IsBusy = true;

			_layoutRoot = new StackLayout();

			Content = _layoutRoot;

			this.IsBusy = false;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (_firstVisit)
			{
				var authPage = new AuthPage();
				await this.Navigation.PushModalAsync(authPage);
				await authPage.AuthCompleteTask;
				_firstVisit = false;
			}

			this.IsBusy = true;;

			var o365 = new O365();
			var meeting = await o365.GetMeeting();

			AppointmentDetails appointment;
			if (false)
			{
				appointment = new AppointmentDetails
				{
					MeetingTitle = "Test Meeting",
					Date = DateTime.Now,
					LocationName = "Conference Room 4",
					Attendees = {
						new Attendee { Name="Jason Young", Email="jayoung@microsoft.com" },
						new Attendee { Name="Graham Elliott", Email="graham.elliott@microsoft.com" }
					}
				};
			}
			else
			{
				appointment = new AppointmentDetails();

				appointment.MeetingTitle = meeting.Subject;

				appointment.Date = DateTime.Parse(meeting.Start.DateTime);
				appointment.Date = DateTime.SpecifyKind(appointment.Date, DateTimeKind.Utc).ToLocalTime();

				var end = DateTime.Parse(meeting.End.DateTime);
				end = DateTime.SpecifyKind(end, DateTimeKind.Utc).ToLocalTime();
				appointment.Duration = end - appointment.Date;

				appointment.LocationName = meeting.Location.DisplayName;

				appointment.Attendees = new System.Collections.Generic.List<Attendee>();
				foreach (var a in meeting.Attendees)
				{
					appointment.Attendees.Add(new Attendee { Name = a.EmailAddress.Name, Email = a.EmailAddress.Address });
				}
			}

			_controller = new MeetingDetailsUI(_layoutRoot, appointment);

			_controller.AttendeeSelected += (a) =>
			{
				this.Navigation.PushAsync(new AttendeeDetailsPage());
			};

			this.IsBusy = false;
		}
	}
}


