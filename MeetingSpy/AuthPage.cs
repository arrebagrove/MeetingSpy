using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetingSpy
{
	public class AuthPage : ContentPage
	{
		private string clientId = "dd122742-c01f-438f-aaac-451ff37ba079";

		string authority = "https://login.microsoftonline.com/common";
		string resource = "https://graph.microsoft.com/";
		string returnUri = "http://meetingspy.com";

		AuthenticationContext authContext = null;

		public static AuthenticationResult AuthResult;

		public IPlatformParameters PlatformParameters { get; set; }

		// Use this to wait on the page to be finished with/closed/dismissed
		public Task AuthCompleteTask { get { return tcs.Task; } }
		TaskCompletionSource<bool> tcs { get; set; }

		public AuthPage()
		{
			tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
			authContext = new AuthenticationContext(authority);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
	
			if (AuthResult == null)
			{
				AuthResult = await GetADALToken(resource);
			}

			await Application.Current.MainPage.Navigation.PopModalAsync();
		}

		private async Task<AuthenticationResult> GetADALToken(string serviceResourceId)
		{
			try
			{
				AuthResult = await authContext.AcquireTokenSilentAsync(serviceResourceId, clientId);
				//authResult = await authContext.AcquireTokenAsync(resource, clientId, new Uri(returnUri), this.PlatformParameters);
			}
			catch (Exception)
			{
				//if (ex.ErrorCode.Equals(AdalError.FailedToAcquireTokenSilently))
				{
					AuthResult = await authContext.AcquireTokenAsync(resource, clientId, new Uri(returnUri), PlatformParameters);
				}
			}

			//Tell the parent page we're done
			tcs.SetResult(true);

			return AuthResult;
		}
	}
}

