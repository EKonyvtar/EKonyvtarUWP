using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    public class SearchResult
    {
        public List<Book> BookList { get; set; }
        public List<Book> Book { get; set; }
    }
}
