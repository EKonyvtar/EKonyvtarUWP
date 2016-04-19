using EKonyvtarUW.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using System.Xml;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EKonyvtarUW.Services
{
    public static class OnlineMekService
    {
        public static string domain = "http://vmek.oszk.hu/";

        public static string StringConvert(string text, string enc = "ISO-8859-2")
        {
            //https://msdn.microsoft.com/en-us/library/windows/desktop/dd317756(v=vs.85).aspx
            //28592 - iso-8859-2
            /*Encoding iso = new Latin2Encoding(); // Encoding.GetEncoding("latin2");
            Encoding utf8 = Encoding.UTF8;
            byte[] utf8bytes = utf8.GetBytes(text);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utf8bytes);
            string msg = iso.GetString(isoBytes);
            return msg;*/
            var converted = text;
            converted = converted.Replace("<br>", "\n");
            converted = TextManipulation.StripHTML(converted);
            converted = new Regex("\\s+(Tartalom|Fülszöveg)\\n\\s+").Replace(converted, "");
            return converted;
        }

        public static async Task<Book> GetBookByUid(string id)
        {
            // "http://vmek.oszk.hu/mobil/konyvoldal.phtml?id=12455";

            if (id.Contains("/"))
            {
                id = id.Split('/')[1];
            }
            var uri = String.Format("http://vmek.oszk.hu/mobil/konyvoldal.phtml?id={0}", id);

            try
            {
                var book = new Book() { Id = id };
                var webGet = new HtmlWeb();
                var document = await webGet.LoadFromWebAsync(uri, new Latin2Encoding());
                var enc = document.Encoding;
                var elements = document.DocumentNode.Descendants();

                book.Title = StringConvert(elements.Where(n => n.GetAttributeValue("class", "").Equals("cim")).First().InnerText);

                // Filling up fulszoveg
                var fulszoveg = elements.Where(n => n.GetAttributeValue("id", "").Equals("fulszoveg")).First().ChildNodes.Where(n => n.Name == "fieldset");
                if (fulszoveg.Count() > 0)
                    book.Contents = StringConvert(fulszoveg.First().InnerHtml);
                if (fulszoveg.Count() > 1)
                    book.Summary = StringConvert(fulszoveg.Last().InnerHtml);


                var cedula = elements.Where(n => n.GetAttributeValue("id", "").Equals("cedula")).First().ChildNodes.Where(n => n.Name == "fieldset");
                book.Abbreviation = StringConvert(cedula.First().InnerHtml);

                var img = elements.Where(n => n.GetAttributeValue("id", "").Equals("footer")).First().ChildNodes[1].ChildNodes.Where(n => n.Name == "img").First();
                book.ThumbnailUrl = new Uri(domain + img.GetAttributeValue("src", ""));

                return book;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }


        public static async Task<Book> GetBookFormatByUri(string bookUri)
        {
            //  "http://mek.oszk.hu/kozoskereso/mobil/mekforma_.php?id=MEK-12455";
            var searchEndpoint = "http://mek.oszk.hu/kozoskereso/mobil/mekforma_.php?id={0}";
            var uri = String.Format(searchEndpoint, bookUri);

            HttpClient client = new HttpClient();
            var json = "";
            try
            {
                json = await client.GetStringAsync(uri);
            }
            catch { }
            client.Dispose();

            var results = new List<Book>();
            json = TextManipulation.StripJson(json);
            JsonArray root = JsonValue.Parse(json).GetArray();
            return null;
        }

        public static async Task<List<Book>> SearchBookAsync(string searchKeyword = "", string searchTitle = "", string searchCreator = "")
        {
            // "http://mek.oszk.hu/kozoskereso/mobil/index.php?alkoto=&cim=&temakor=horthy";
            var searchEndpoint = "http://mek.oszk.hu/kozoskereso/mobil/index.php?alkoto={0}&cim={1}&temakor={2}";
            var uri = String.Format(searchEndpoint, searchCreator, searchTitle, searchKeyword);

            HttpClient client = new HttpClient();
            var json = "";
            try
            {
                json = await client.GetStringAsync(uri);
            }
            catch { }
            client.Dispose();

            var results = new List<Book>();
            json = TextManipulation.StripJson(json);
            JsonArray root = JsonValue.Parse(json).GetArray();
            for (uint i = 0; i < root.Count; i++)
            {
                string collection = root.GetObjectAt(i).GetNamedString("collection");
                string id = root.GetObjectAt(i).GetNamedString("id");
                string url = root.GetObjectAt(i).GetNamedString("URL");
                string title = root.GetObjectAt(i).GetNamedString("title");
                string subtitle = root.GetObjectAt(i).GetNamedString("subtitle");
                string creators = root.GetObjectAt(i).GetNamedString("creators");
                string thumbnailUrl = root.GetObjectAt(i).GetNamedString("thumbnailURL");

                if (id == "0") continue;

                //TODO: solve paging
                //http://mek.oszk.hu/kozoskereso/mobil/mek_epa_dka_kereso/build/
                //TODO: Imagefeed 

                var book = new Book()
                {
                    Id = id,
                    Url = url,
                    Collection = collection,
                    Title = title,
                    SubTitle = subtitle,
                    Creators = creators,
                    ThumbnailUrl = new Uri(thumbnailUrl)
                };
                results.Add(book);

            };
            return results;
        }
    }
}
