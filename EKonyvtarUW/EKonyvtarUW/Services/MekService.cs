//using SQLite.Net.Async;
using EKonyvtarUW.Models;
using SQLite;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EKonyvtarUW.Services
{
    public class MekService
    {
        public static async Task<Book> GetBookByUrlId(string urlId)
        {
            var onlineBook = await OnlineMekService.GetBookByUrlId(urlId);
            return onlineBook;
        }

        public static async Task<List<Book>> SearchBookAsync(string searchKeyword = "")
        {
            var fault = false;
            var result = new List<Book>();

            try
            {
                var onlineKeyWord = await OnlineMekService.SearchBookAsync(searchKeyword, "", "");
                if (onlineKeyWord != null)
                    result.AddRange(onlineKeyWord);

            }
            catch
            {
                fault = true;
            }

            //try
            //{
            //    var onlineTitle = await OnlineMekService.SearchBookAsync("", searchKeyword, "");
            //    if (onlineTitle != null)
            //        result.AddRange(onlineTitle);

            //}
            //catch
            //{
            //    fault = true;
            //}

            try
            {
                var onlineCreator = await OnlineMekService.SearchBookAsync("", "", searchKeyword);
                if (onlineCreator != null)
                    result.AddRange(onlineCreator);

            }
            catch
            {
                fault = true;
            }

            try
            {
                var offline = await LocalMekService.SearchBookAsync(searchKeyword);
                if (offline != null)
                    result.AddRange(offline);
            }
            catch
            {
                fault = true;
            }

            if (result.Count > 1)
            {
                result = result.GroupBy(b => b.UrlId).Select(grp => grp.First()).ToList();
            }
            return result;
        }
    }
}
