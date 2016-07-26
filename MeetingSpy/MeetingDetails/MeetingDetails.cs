using System;
using Xamarin.Forms;

namespace MeetingSpy
{
	public class MeetingDetails : ContentPage
	{
		private StackLayout _layoutRoot;

		private bool _firstVisit = true;


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

			var appointment = new AppointmentDetails
			{
				MeetingTitle = "Test Meeting",
				Date = DateTime.Now,
				LocationName = "Conference Room 4",
				Attendees = {
					new Attendee { Name="Jason Young", Email="jayoung@microsoft.com" },
					new Attendee { Name="Graham Elliott", Email="graham.elliott@microsoft.com" }
				}
			};

			var meetingDetails = new MeetingDetailsUI(_layoutRoot, appointment);

			this.IsBusy = false;
		}
	}
}


