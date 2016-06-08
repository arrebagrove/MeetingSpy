using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MeetingSpy
{
	public static class LinkedIn
	{
		public static async Task<List<SearchResult>> Search(string firstName, string lastName)
		{
			const string linkedInSearchUrl = "https://www.linkedin.com/pub/dir/?first={0}&last={1}";

			var firstNameEncoded = System.Net.WebUtility.UrlEncode(firstName);
			var LastNameEncoded = System.Net.WebUtility.UrlEncode(lastName);

			var http = new System.Net.Http.HttpClient();

			var actualSearchUrl = string.Format(linkedInSearchUrl, firstNameEncoded, LastNameEncoded);

			var response = await http.GetStringAsync(actualSearchUrl);

			if (response.Length == 0)
			{
				throw new Exception("Zero length response received from LinkedIn");
			}

			var length = response.Length;

			var doc = new HtmlDocument();
			doc.LoadHtml(response);

			var searchResults = new List<SearchResult>();

			var profiles = doc.DocumentNode.Descendants().Where(x => x.HasAttribute("class") && x.Attributes["class"].Value == "profile-card");

			foreach (var profile in profiles)
			{
				var searchResult = new SearchResult();

				//<p class="headline">Technical Evangelist at Microsoft</p>
				var titleNode = profile.Descendants()
					.FirstOrDefault(x => x.HasAttribute("class") && x.Attributes["class"].Value == "headline");
				if (titleNode != null)
				{
					searchResult.Title = titleNode.InnerText;
				}

				var locationNode = profile.Descendants()
					.FirstOrDefault(x => x.InnerText != null && x.InnerText == "Location");
				if (locationNode != null)
				{
					locationNode = locationNode.NextSibling;
				}
				if (locationNode != null)
				{
					searchResult.Location = locationNode.InnerText;
				}

				var educationNode = profile.Descendants()
					.FirstOrDefault(x => x.InnerText != null && x.InnerText == "Education");
				if (educationNode != null)
				{
					educationNode = educationNode.NextSibling;
				}
				if (educationNode != null)
				{
					searchResult.Education = educationNode.InnerText;
				}


				var fullProfileNode = profile.Descendants()
					.FirstOrDefault(x => x.InnerText != null && x.InnerText == "View Full Profile");
				if (fullProfileNode != null)
				{
					searchResult.ProfileUrl = fullProfileNode.Attributes["data-href"].Value;
				}

				searchResult.Name = firstName + " " + lastName; //default to what we know
				var fullNameNode = profile.Descendants()
				                          .FirstOrDefault(x => x.Name == "h3");
				if (fullNameNode != null)
				{
					if (fullNameNode.FirstChild != null)
					{
						searchResult.Name = fullNameNode.FirstChild.InnerText;
					}
				}

				var pastNode = profile.Descendants()
					.FirstOrDefault(x => x.InnerText != null && x.InnerText == "Past");
				if (pastNode != null)
				{
					pastNode = pastNode.NextSibling;
				}
				if (pastNode != null)
				{
					searchResult.LastJob = pastNode.InnerText;
				}

				var photoNode = profile.Descendants()
				                       .FirstOrDefault(x => x.Name == "img");
				if (photoNode != null && photoNode.Attributes["src"] != null)
				{
					searchResult.ProfilePhotoUrl = photoNode.Attributes["src"].Value;
					searchResult.ProfilePhotoUrl = searchResult.ProfilePhotoUrl.Replace("shrink_100_100/", "");
				}

				searchResults.Add(searchResult);
			}

			return searchResults;
		}

	}

	public class SearchResult
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Location { get; set; }
		public string Education { get; set; }
		public string ProfileUrl { get; set; }
		public string LastJob { get; set; }
		public string ProfilePhotoUrl { get; set; }
	}

}

