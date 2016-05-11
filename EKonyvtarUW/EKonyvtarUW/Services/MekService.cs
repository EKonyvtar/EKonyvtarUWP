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
            //var offlineBook = await LocalMekService.GetBookByUrlId(urlId);
            //if (offlineBook != null)
            //    return offlineBook;

            var onlineBook = await OnlineMekService.GetBookByUrlId(urlId);
            return onlineBook;
        }

        public static async Task<List<Book>> SearchBookAsync(string searchKeyword = "", string searchTitle = "", string searchCreator = "")
        {
            List<Task<List<Book>>> bookTasks = new List<Task<List<Book>>>();
            bookTasks.Add(LocalMekService.SearchBookAsync(searchKeyword));
            bookTasks.Add(OnlineMekService.SearchBookAsync(searchKeyword, searchTitle, searchCreator));
            Task<List<Book>> firstFinishedTask = await Task.WhenAny(bookTasks);
            return firstFinishedTask.Result;
        }
    }
}
