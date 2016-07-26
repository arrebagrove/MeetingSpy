using System;
using MeetingSpy.Models;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetingSpy
{
	public class O365
	{
		string graphEndpoint = "https://graph.microsoft.com/v1.0/me/events";

		private AuthenticationResult _authResult;

		public O365()
		{
		}

		public async Task<OutlookItem> GetMeeting()
		{
			try
			{
				var graphserviceClient = new GraphServiceClient(
					new DelegateAuthenticationProvider(
						(requestMessage) =>
						{
					requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", AuthPage.AuthResult.AccessToken);

							return Task.FromResult(0);
						}));

				var request = graphserviceClient.Me.Events.Request();
				var events = await request.GetAsync();

				return events[0];
			}
			catch (ServiceException)
			{
				return null;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error loading current meeting: " + ex.Message);
				return null;
			}
		}



		private async Task DoHttpRequest(AuthenticationResult authResult)
		{
			// use this token to get the list of contacts
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthPage.AuthResult.AccessToken);
			client.DefaultRequestHeaders.Add("Accept", "application/json");

			using (HttpResponseMessage response = await client.GetAsync(new Uri(graphEndpoint)))
			{
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					//this.loginBtn.IsVisible = false;
					//this.BindingContext = ContactViewModel.CreateInstance(content);
				}
			}
		}


	}
}

