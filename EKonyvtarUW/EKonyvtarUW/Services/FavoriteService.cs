using EKonyvtarUW.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using System.Linq;
using System.Threading;

namespace EKonyvtarUW.Services
{
    public static class FavoriteService
    {
        private const string favoriteFileName = "favorites.json";
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private static List<Book> _favorites = null;
        public static List<Book> Favorites
        {
            get
            {
                CheckInitialized();

                return _favorites;
            }
        }

        public static bool IsBookFavorited(Book book)
        {
            CheckInitialized();

            var filtered = GetBook(book);
            return (filtered != null);
        }

        private static void CheckInitialized()
        {
            if (_favorites == null)
            {
                LoadFavorites();
            }
        }

        public static async Task AddBook(Book book)
        {
            CheckInitialized();

            if (!IsBookFavorited(book))
                Favorites.Add(book);

            SaveFavorites();
        }

        public static void RemoveBook(Book book)
        {
            CheckInitialized();

            var filtered = GetBook(book);
            if (filtered != null)
                Favorites.Remove(filtered);
            SaveFavorites();
        }

        public static Book GetBook(Book book)
        {
            CheckInitialized();

            return Favorites?.Where(t => t.UrlId == book.UrlId).FirstOrDefault();
        }

        public static async Task<List<Book>> SearchFavoriteAsync(string text)
        {
            CheckInitialized();

            if (Favorites == null)
                await LoadFavorites();


            var filtered = Favorites?.Where(t =>
                t.Title.Contains(text) ||
                t.Summary.Contains(text) ||
                t.Labels.Contains(text)
            );
            return filtered.ToList();
        }

        private static async Task LoadFavorites()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                //CheatSheet: https://msdn.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
                StorageFile favoriteFile = await ApplicationData.Current.LocalFolder.GetFileAsync(favoriteFileName);
                string json = await Windows.Storage.FileIO.ReadTextAsync(favoriteFile);
                _favorites = JsonConvert.DeserializeObject<List<Book>>(json);
            }
            catch (UnauthorizedAccessException)
            {
                // Swallow unathorized exception
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception)
            {
                //Unknown exception yet

            }
            finally
            {
                semaphoreSlim.Release();
            }

            if (_favorites == null)
            {
                _favorites = new List<Book>();
            }
        }

        private static async void SaveFavorites()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                var json = JsonConvert.SerializeObject(_favorites);
                // CheatSheet: https://msdn.microsoft.com/en-us/windows/uwp/app-settings/store-and-retrieve-app-data
                StorageFile favoriteFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(favoriteFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(favoriteFile, json);

            }
            catch (Exception)
            {
                // File not found exception 
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
