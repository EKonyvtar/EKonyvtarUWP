using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    [Table("VIEW_BOOKS")]
    public class Book
    {
        private static string mekUrl = "http://mek.oszk.hu/{0}/borito.jpg";

        [Column("_id")]
        public string DbId { get; set; }

        [Column("url")]
        public string UrlId { get; set; }
        public string Url { get; set; }

        private Uri _ThumbnailUrl = null;
        public Uri ThumbnailUrl
        {
            get
            {
                if (_ThumbnailUrl != null)
                    return _ThumbnailUrl;

                return new Uri(String.Format(mekUrl, UrlId));
            }
            set
            {
                _ThumbnailUrl = value;
            }
        }

        [Column("title")]
        public string Title { get; set; }

        [Column("creator_name")]
        public string Creators { get; set; }

        [Column("subtitle")]
        public string SubTitle { get; set; }

        [Column("year")]
        public string Year { get; set; }

        //[Column("place")]
        //public string Place { get; set; }

        public string Contents { get; set; }

        public string Summary { get; set; }


        public string Collection { get; set; }


        public string Abbreviation { get; set; }
        public List<string> Media { get; set; }
        public string ContentUri
        {
            get
            {
                //return Media.FirstOrDefault();
                return String.Format("https://docs.google.com/gview?url={0}&embedded=true", Media.FirstOrDefault());
                //return String.Format("http://docs.google.com/viewer?url={0}", Media.FirstOrDefault());
            }
        }

        //public string BookTypeName { get; set; }
        //public string Published { get; set; }
        //public string TopicName { get; set; }

        //TODO: reviews
        //TODO: Similar
    }
}
