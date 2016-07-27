using Xamarin.Forms;

namespace MeetingSpy
{
    public partial class AttendeeDetailsPage : ContentPage
    {
		private Attendee _attendee;

        public AttendeeDetailsPage(Attendee attendee)
        {
			this.InitializeComponent();

			_attendee = attendee;
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var parts = _attendee.Name.Split(" ".ToCharArray());

			IsBusy = true;
			var linkedInAttendees = await LinkedIn.Search(parts[0], parts[1]);
			IsBusy = false;

			DetailLabel.Text = "";
			foreach (var attendee in linkedInAttendees)
			{
				AddText("Name: " + attendee.Name);
				AddText("Title: " + attendee.Title);
				AddText("Location: " + attendee.Location);
				AddText("Last Job: " + attendee.LastJob);
				AddText("Profile Photo: " + attendee.ProfilePhotoUrl);
				AddText("Education: " + attendee.Education);

				AddText("-------------");
			}
		}

		private void AddText(string newText)
		{
			DetailLabel.Text += newText + "\n";
		}
    }
}
