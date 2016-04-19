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
        public static async Task<Book> GetBookByUid(string uid)
        {
            List<Task<Book>> bookTasks = new List<Task<Book>>();
            //bookTasks.Add(LocalMekService.GetBookByUid(uid));
            bookTasks.Add(OnlineMekService.GetBookByUid(uid));
            Task<Book> firstFinishedTask = await Task.WhenAny(bookTasks);
            return firstFinishedTask.Result;
        }

        public static async Task<List<Book>> SearchBookAsync(string searchKeyword = "", string searchTitle = "", string searchCreator = "")
        {
            List<Task<List<Book>>> bookTasks = new List<Task<List<Book>>>();
            //bookTasks.Add(LocalMekService.GetBookByUid(uid));
            bookTasks.Add(OnlineMekService.SearchBookAsync(searchKeyword, searchTitle, searchCreator));
            Task<List<Book>> firstFinishedTask = await Task.WhenAny(bookTasks);
            return firstFinishedTask.Result;
        }
    }
}
