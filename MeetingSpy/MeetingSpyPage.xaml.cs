using MeetingSpy.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeetingSpy
{
	public partial class MeetingSpyPage : ContentPage
	{
        private string clientId = "dd122742-c01f-438f-aaac-451ff37ba079";
        string graphEndpoint = "https://graph.microsoft.com/v1.0/me/contacts";
        string authority = "https://login.microsoftonline.com/common";
        string resource = "https://graph.microsoft.com/";
        string returnUri = "http://meetingspy.com";

        public IPlatformParameters PlatformParameters { get; set; }

        AuthenticationContext authContext = null;

        public MeetingSpyPage()
		{
			InitializeComponent();

            authContext = new AuthenticationContext(authority);
        }

        async void OnClicked(object sender, EventArgs args)
        {
            AuthenticationResult authResult = await GetADALToken(resource);
            await DoHttpRequest(authResult);
        }

        private async Task<AuthenticationResult> GetADALToken(string serviceResourceId)
        {
            AuthenticationResult authResult = null;

            try
            {
                authResult = await authContext.AcquireTokenSilentAsync(serviceResourceId, clientId);
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode.Equals(AdalError.FailedToAcquireTokenSilently))
                {
                    authResult = await authContext.AcquireTokenAsync(resource, clientId, new Uri(returnUri), this.PlatformParameters);
                }
            }
            catch (Exception ex)
            {
                var i = 0;
            }

            return authResult;
        }

        private async Task DoHttpRequest(AuthenticationResult authResult)
        {
            // use this token to get the list of contacts
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authResult.AccessToken);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            using (HttpResponseMessage response = await client.GetAsync(new Uri(graphEndpoint)))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    this.loginBtn.IsVisible = false;
                    this.BindingContext = ContactViewModel.CreateInstance(content);
                }
            }
        }
    }
}

