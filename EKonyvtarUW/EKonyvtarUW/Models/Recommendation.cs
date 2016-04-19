using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    public class Recommendation
    {
        public String Title { get; set; }
        public String Link { get; set; }
        public String Summary { get; set; }

        public String Text { get; set;  }
        public String ThumbnailUrl { get; set; }

        public Book ToBook()
        {
            return new Book()
            {
                Title = this.Title,
                ThumbnailUrl = new Uri(this.ThumbnailUrl),
                Abbreviation = Text
            };
        }
    }
}
