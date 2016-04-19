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
        
        public static async Task<List<Recommendation>> GetMekFeedAsync()
        {
            var feed = await GetFeedAsync("http://mek.oszk.hu/mek2.rss");
            var list = feed.Select(f => new Recommendation()
            {
                Title = f.Title.Text,
                Summary = f.Summary.Text,
                Text = TextManipulation.StripHTML(f.Summary.Text).Replace(("^"+ f.Title.Text),""),
                ThumbnailUrl = TextManipulation.GetImageUrl(f.Summary.Text),
                Link = f.Links[0].Uri.ToString()

            }).ToList<Recommendation>();
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

                        //RssFeedModel feedModel = new RssFeedModel(feed.Title.Text, feed.Subtitle != null ? feed.Subtitle.Text : "", new Uri(_url), null);

                        return feed.Items;
                        /*(feed.Items.Select(f =>
                                 new RssArticleModel(f.Title.Text,
                                     f.Summary != null ? Regex.Replace(Regex.Replace(f.Summary.Text, "\\&.{0,4}\\;", string.Empty), "<.*?>", string.Empty) : "",
                                     f.Authors.Select(a => a.NodeValue).FirstOrDefault(),
                                     f.ItemUri != null ? f.ItemUri : f.Links.Select(l => l.Uri).FirstOrDefault()
                                     )));*/

                        //if (FeedsList.Contains(feedModel))
                        // {
                        //   FeedsList.Remove(feedModel);
                        //}

                        //FeedsList.Add(feedModel);
                    }
                }
            }
            catch (Exception)
            {
                //InvalidRSSFeedMessageVisibility = Visibility.Visible;
            }
            return null;
        }
    }
}
