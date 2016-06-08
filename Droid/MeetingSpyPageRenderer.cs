using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MeetingSpy;
using MeetingSpy.Droid;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MeetingSpyPage), typeof(MeetingSpyPageRenderer))]
namespace MeetingSpy.Droid
{
    public class MeetingSpyPageRenderer : PageRenderer
    {
        MeetingSpyPage page;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            page = e.NewElement as MeetingSpyPage;

            // provides the concrete implementation of IPlatformParameters
            page.PlatformParameters = new PlatformParameters((Activity)Xamarin.Forms.Forms.Context); ;
        }
    }
}