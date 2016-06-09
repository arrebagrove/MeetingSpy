using Xamarin.Forms;

namespace MeetingSpy
{
    public partial class AttendeeDetailsPage : ContentPage
    {
        public AttendeeDetailsPage()
        {
            InitializeComponent();
        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			IsBusy = true;
			var linkedInAttendees = await LinkedIn.Search("Richard", "Custance");
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
