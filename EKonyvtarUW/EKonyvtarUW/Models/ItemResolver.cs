using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    public class ItemResolver
    {
        public static ItemResolver Resolve(string id)
        {
            return new ItemResolver(id);
        }

        public Type ItemType { get; private set; }
        public string UrlId { get; private set; }
        public string Url { get; private set; }


        public ItemResolver(string id)
        {
            ItemType = typeof(Book);
            Match matches = null;

            if (Regex.IsMatch(id, "^https?://"))
                Url = id;

            matches = Regex.Match(id, "(\\d+[\\\\/]\\d+)");
            if (matches != null)
                UrlId = matches.Groups[1].Value;


            if (Regex.IsMatch(id,"\\d"))
            {
                //TODO: resolve by ID only
            }
        }
    }
}
