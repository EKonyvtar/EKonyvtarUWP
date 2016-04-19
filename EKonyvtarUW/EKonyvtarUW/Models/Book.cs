using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    [Table("book")]
    public class Book
    {
        [Column("_id")]
        public string Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("subtitle")]
        public string SubTitle { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("year")]
        public string Year { get; set; }

        [Column("place")]
        public string Place { get; set; }

        public string Contents { get; set; }
        public string Summary { get; set; }

        public string BookTypeName { get; set; }

        public string TopicName { get; set; }

        public string Collection { get; set; }
        public string Creators { get; set; }
        public string Published { get; set; }

        public Uri ThumbnailUrl { get; set; }

        public string Abbreviation { get; set; }
        public List<string> Media { get; set; }



        //TODO: reviews
        //TODO: Similar
    }
}
