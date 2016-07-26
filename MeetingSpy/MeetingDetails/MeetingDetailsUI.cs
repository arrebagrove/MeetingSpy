using System;
using System.Linq;
using Xamarin.Forms;

namespace MeetingSpy
{
	public class MeetingDetailsUI
	{
		public MeetingDetailsUI(StackLayout root, AppointmentDetails apptDetails)
		{
			root.Children.Clear();

			var titleLabel = new Label();
			titleLabel.Text = apptDetails.MeetingTitle;
			titleLabel.FontAttributes = FontAttributes.Bold;

			var dateLabel = new Label();
			if (apptDetails.Date != new DateTime())
			{
				dateLabel.Text = apptDetails.Date.ToString("D");
			}

			var timeLabel = new Label();
			timeLabel.Text = apptDetails.Date.ToString("t");
			timeLabel.Text += " → ";
			timeLabel.Text += apptDetails.Date.Add(apptDetails.Duration).ToString("t");

			if (apptDetails.Duration.TotalMinutes < 60)
			{
				timeLabel.Text += string.Format(" ({0:%m} minutes)", apptDetails.Duration);
			}
			else if (apptDetails.Duration.TotalHours % 1 == 0)
			{
				timeLabel.Text += string.Format(" ({0:%h} hours)", apptDetails.Duration);
			}
			else {
				timeLabel.Text += string.Format(" ({0:%h} hours, {0:%m} minutes)", apptDetails.Duration);
			}

			var peopleCells = apptDetails.Attendees.Select(
				a => new TextCell
				{
					Text = a.Name + " >"
				}
			);

			var details = new ViewCell();
			details.View = new StackLayout()
			{
				Padding = new Thickness(15),
				Children = { titleLabel, dateLabel, timeLabel }
			};

			var detailsTable = new TableView
			{
				Intent = TableIntent.Form,
				HasUnevenRows = true,
				Root = new TableRoot("root") {
				new TableSection {
					details
				},
				new TableSection {
					new TextCell() { Text = apptDetails.LocationName }
				},
				new TableSection {
					peopleCells
				}
			}
			};
			root.Children.Add(detailsTable);
		}
	}
}

