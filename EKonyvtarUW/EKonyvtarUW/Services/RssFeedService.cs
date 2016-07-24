using EKonyvtarUW.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Syndication;
using EKonyvtarUW.Models;

namespace EKonyvtarUW.Services
{
    public static class RssFeedService
    {

        public static async Task<List<Book>> GetMekFeedAsync()
        {
            var feed = await GetFeedAsync("http://mek.oszk.hu/mek2.rss");
            var list = new List<Book>();
            list = feed?.Select(f => new Recommendation()
            {
                Title = f.Title.Text,
                Summary = f.Summary.Text,
                Abbreviation = TextManipulation.StripHTML(f.Summary.Text).Replace(("^" + f.Title.Text), ""),
                ThumbnailUrl = TextManipulation.GetImageUrl(f.Summary.Text),
                UrlId = f.Links[0].Uri.ToString()

            }.ToBook()).ToList<Book>();

            return list;

        }

        public static async Task<IList<SyndicationItem>> GetFeedAsync(string url)
        {
            try
            {
                SyndicationClient client = new SyndicationClient();

                Uri myUri;
                if (Uri.TryCreate(url, UriKind.Absolute, out myUri))
                {
                    if ((myUri.Scheme == "http" || myUri.Scheme == "https"))
                    {
                        SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(url));
                        return feed.Items;
                    }
                }
            }
            catch (Exception ex)
            {
                //InvalidRSSFeedMessageVisibility = Visibility.Visible;
            }
            return null;
        }
    }
}
