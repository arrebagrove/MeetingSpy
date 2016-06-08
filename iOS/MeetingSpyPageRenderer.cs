using MeetingSpy;
using MeetingSpy.iOS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MeetingSpyPage), typeof(MeetingSpyPageRenderer))]
namespace MeetingSpy.iOS
{
    public class MeetingSpyPageRenderer : PageRenderer
    {
        // keeps a reference to the page that this is being applied to
        MeetingSpyPage page;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            page = e.NewElement as MeetingSpyPage;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // provides the concrete implementation of IPlatformParameters
            page.PlatformParameters = new PlatformParameters(this); ;
        }
    }
}
