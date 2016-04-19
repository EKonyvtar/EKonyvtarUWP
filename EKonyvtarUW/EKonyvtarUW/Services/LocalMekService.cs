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
    public class LocalMekService
    {
        const string dbFile = "mek.sqlite";

        public static async Task CopyDatabaseFile()
        {
            //Test file if exists
            try
            {
                StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/mek.sqlite"));
                await file2.CopyAsync(ApplicationData.Current.LocalFolder, dbFile);
            }
            catch
            {
                //TODO: proper catch // File is already copied over
            }
        }


        private static SQLiteConnection DbConnection
        {
            get
            {
                var db_path = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbFile);
                if (!File.Exists(db_path))
                    CopyDatabaseFile().RunSynchronously();


                return new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), db_path);
            }
        }

        public static async Task<List<string>> GetCategories(string uniqueId = null)
        {
            //TODO: cache toplevel 
            using (var db = DbConnection)
            {
                var topicList = db.Table<Topic>().Where(t => t.ParentId == uniqueId).Select(t => t.Name).ToList<string>();
                return topicList;
            }

        }

        public static async Task<Book> GetBookByUid(string uriId)
        {
            using (var db = DbConnection)
            {
                var topicList = db.Table<Book>().Where(t => t.Url == uriId).First();
                return topicList;
            }
        }

        public static async Task<Book> GetBook(string uniqueId)
        {
            //TODO: cache toplevel
            using (var db = DbConnection)
            {
                var topicList = db.Table<Book>().Where(t => t.Id == uniqueId).First();
                return topicList;
            }
        }

        public static async Task<List<SearchEntry>> FindBook(string text)
        {
            //TODO: cache toplevel
            using (var db = DbConnection)
            {
                var topicList = db.Table<SearchEntry>().Where(t => t.Title.Contains(text)).ToList<SearchEntry>();
                return topicList;
            }
        }
    }
}
