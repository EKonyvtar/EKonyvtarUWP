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
        public Type ItemType { get; private set; }
        public string Uri { get; private set; }
        public string Url { get; private set; }


        public ItemResolver(string url)
        {
            Url = url;

            var matches = Regex.Match(url, "(\\d+[\\\\/]\\d+)");
            if(matches != null)
            {
                ItemType = typeof(Book);
                Uri = matches.Groups[1].Value;
            }
        }
    }
}
