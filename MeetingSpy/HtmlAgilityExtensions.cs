using System.Linq;
using HtmlAgilityPack;

namespace MeetingSpy
{
	public static class HtmlAgilityExtensions
	{

		public static bool HasAttribute(this HtmlNode element, string attributeName)
		{
			if (element.Attributes != null && element.Attributes.Count > 0)
			{
				if (element.Attributes.Where(x => x.Name != null && x.Name == attributeName).Count() > 0)
				{
					return true;
				}
			}

			return false;
		}

	}
}

