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
            converted = new Regex("\\s+(Tartalom|Fülszöveg|Leírás)\\n\\s+").Replace(converted, "");
            converted = new Regex(@"\n+").Replace(converted, "\n");
            converted = converted.Trim();
            return converted;
        }

        public static async Task<Book> GetBookByUrlId(string urlId)
        {
            // "http://vmek.oszk.hu/mobil/konyvoldal.phtml?id=12455";
            var shortUrlId = urlId;

            if (urlId.Contains("/")) shortUrlId = urlId.Split('/')[1];

            var uri = String.Format("http://vmek.oszk.hu/mobil/konyvoldal.phtml?id={0}", shortUrlId);

            try
            {
                var book = new Book() { UrlId = shortUrlId };
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
                book.Labels = StringConvert(cedula.First().InnerHtml);

                var img = elements.Where(n => n.GetAttributeValue("id", "").Equals("footer")).First().ChildNodes[1].ChildNodes.Where(n => n.Name == "img").First();
                book.ThumbnailUrl = new Uri(domain + img.GetAttributeValue("src", ""));

                //TODO populate list in a more smart way
                HttpClient httpClient = new HttpClient();
                book.Media = new List<string>();
                foreach (var ext in new string[] { "{1}.pdf", "{1}.html", "{1}.htm", "pdf/{1}.pdf", "pdf/{1}_1.pdf", "pdf/{1}_2.pdf", "{1}.doc" }) //"{1}.rtf"
                {
                    if (book.Media.Count > 2) break;
                    var mediaUrl = String.Format(("http://mek.oszk.hu/{0}/" + ext), urlId, shortUrlId);
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, new Uri(mediaUrl));
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        book.Media.Add(mediaUrl);
                }

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
            // DKA: http://mek.oszk.hu/kozoskereso/mobil/mek_epa_dka_kereso/build/
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
                try
                {
                    string id = root.GetObjectAt(i).GetNamedString("id");
                    string collection = root.GetObjectAt(i).GetNamedString("collection");

                    if (id == "0" || collection.ToLower() != "mek") continue;

                    string url = root.GetObjectAt(i).GetNamedString("URL");
                    string title = root.GetObjectAt(i).GetNamedString("title");
                    string subtitle = root.GetObjectAt(i).GetNamedString("subtitle");
                    string creators = root.GetObjectAt(i).GetNamedString("creators");
                    string thumbnailUrl = root.GetObjectAt(i).GetNamedString("thumbnailURL");

                    //TODO: solve paging

                    var book = new Book()
                    {
                        UrlId = ItemResolver.Resolve(url).UrlId,
                        Url = url,
                        Title = title,
                        SubTitle = subtitle,
                        Creators = creators,
                        ThumbnailUrl = new Uri(thumbnailUrl)
                    };
                    results.Add(book);
                }
                catch (Exception ex)
                {
                    //Swallow to survive populating results
                }

            };
            return results;
        }

        public static async Task<List<Book>> SearchBookAsync2(string searchKeyword = "", string searchTitle = "", string searchCreator = "")
        {
            // "http://vmek.oszk.hu/html/vgi/vkereses/mobilkereses.phtml?szerzo=&cim=&tema=horthy&formatum=%25#_talalat - 0";
            // TODO: paging
            var searchEndpoint = "http://vmek.oszk.hu/html/vgi/vkereses/mobilkereses.phtml?szerzo={0}&cim={1}&tema={2}&formatum=%25";
            var uri = String.Format(searchEndpoint, searchCreator, searchTitle, searchKeyword);

            var results = new List<Book>();
            try
            {
                var webGet = new HtmlWeb();
                var document = await webGet.LoadFromWebAsync(uri);
                var elements = document.DocumentNode.Descendants();
                var books = elements.Where(n => n.GetAttributeValue("class", "").Equals("sor"));

                foreach (var row in books)
                {
                    var b = new Book();
                    try
                    {
                        //TODO fill
                        /*
                         <li class="sor">[09484]<a target="_webapp" href=/mobil/konyvoldal.phtml?id=9484&tip=gyors&offset=0&szerzo=&cim=&tema=horthy&formatum=%>
                            <img align="right" src=/09400/09484/borito.jpg height="40" />
                            <span class="szogletes">[3]</span> A Zürichi Magyar Történelmi Egyesület és a Zrínyi Miklós Nemzetvédelmi Egyetem Kossuth Lajos Hadtudományi Kar közös tudományos tanácskozása  </a>
                            <div class="formatum"> PDF</div>
                          </li>
                         */
                    }
                    finally
                    {
                    }
                    results.Add(b);
                }

            }
            catch { }
            return results;
        }
    }
}
