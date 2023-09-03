using EKonyvtarUW.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

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

            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                result = result.Take(30).ToList();
            }
            return result;
        }
    }
}
