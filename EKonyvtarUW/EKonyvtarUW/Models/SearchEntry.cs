using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    [Table("VIEW_BOOKS")]
    public class SearchEntry
    {
        [Column("_id")]
        public string Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("subtitle")]
        public string SubTitle { get; set; }

        [Column("url")]
        public string UrlId { get; set; }
    }
}
