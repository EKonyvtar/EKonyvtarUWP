using EKonyvtarUW.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using System.Linq;

namespace EKonyvtarUW.Services
{
    public class FavoriteService
    {

        static FavoriteService()
        {
            LoadFavorites();
        }

        private static string favoriteFileName = "favorites.json";

        private static List<Book> _favorites = null;
        public static List<Book> Favorites
        {
            get
            {
                if (_favorites == null)
                    LoadFavorites();

                return _favorites;
            }
        }

        public static bool IsBookFavorited(Book book)
        {
            var filtered = GetBook(book);
            return (filtered != null);
        }

        public static void AddBook(Book book)
        {
            //TODO: save stripped
            if (!IsBookFavorited(book))
                _favorites.Add(book);

            SaveFavorites();
        }

        public static void RemoveBook(Book book)
        {
            var filtered = GetBook(book);
            if (filtered != null)
                _favorites.Remove(filtered);
            SaveFavorites();
        }

        public static Book GetBook(Book book)
        {
            return Favorites?.Where(t => t.UrlId == book.UrlId).FirstOrDefault();
        }

        public static async Task<List<Book>> SearchFavoriteAsync(string text)
        {
            if (Favorites == null)
                await LoadFavorites();

            var filtered = Favorites?.Where(t =>
                t.Title.Contains(text) ||
                t.Summary.Contains(text) ||
                t.Abbreviation.Contains(text)
            );
            return filtered.ToList();
        }

        private static async Task LoadFavorites()
        {
            try
            {
                //CheatSheet: https://msdn.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
                StorageFile favoriteFile = await ApplicationData.Current.LocalFolder.GetFileAsync(favoriteFileName);
                string json = await Windows.Storage.FileIO.ReadTextAsync(favoriteFile);
                _favorites = JsonConvert.DeserializeObject<List<Book>>(json);
            }
            catch (UnauthorizedAccessException uex)
            {
                // Swallow unathorized exception
            }
            catch (FileNotFoundException)
            {
                _favorites = new List<Book>();
                SaveFavorites(); //Create the empty file
            }
            catch (Exception ex)
            {
                //Unknown exception yet
                _favorites = new List<Book>();
            }
        }

        private static async void SaveFavorites()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_favorites);
                // CheatSheet: https://msdn.microsoft.com/en-us/windows/uwp/app-settings/store-and-retrieve-app-data
                StorageFile favoriteFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(favoriteFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(favoriteFile, json);

            }
            catch (Exception ex)
            {
                // File not found exception 
            }
        }
    }
}
