using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Net.Http;
using System.IO.Compression;

namespace MeetingSpy
{
	public static class LinkedIn
	{
		public static async Task<List<SearchResult>> Search(string firstName, string lastName)
		{
			const string linkedInSearchUrl = "https://www.linkedin.com/pub/dir/?first={0}&last={1}";
			const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

			var firstNameEncoded = System.Net.WebUtility.UrlEncode(firstName);
			var LastNameEncoded = System.Net.WebUtility.UrlEncode(lastName);



			var actualSearchUrl = string.Format(linkedInSearchUrl, firstNameEncoded, LastNameEncoded);

			var response = await GetResponse(actualSearchUrl);

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

		//http://stackoverflow.com/questions/15026953/httpclient-request-like-browser
		private static async Task<string> GetResponse(string url)
		{
			var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

			var response = await httpClient.GetAsync(new Uri(url));

			response.EnsureSuccessStatusCode();
			using (var responseStream = await response.Content.ReadAsStreamAsync())
			using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
			using (var streamReader = new StreamReader(decompressedStream))
			{
				return streamReader.ReadToEnd();
			}
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

