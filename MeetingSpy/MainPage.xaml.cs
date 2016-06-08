using MeetingSpy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MeetingSpy
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var meetingVM = new MeetingViewModel() {
                Meeting = new Meeting() { Title = "Sync Week", StartTime = "6/6/2016", EndTime = "6/10/2016", Location = "Redmond" }
            };

            this.BindingContext = meetingVM;
        }
    }
}
