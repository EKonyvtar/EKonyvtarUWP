
using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EKonyvtarUW.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private const string PAGE_FAVORITE = "ekonyvtar:page:favorite";

        private readonly INavigationService _navigationService;
        public NotifyTaskCompletion<List<Book>> Favorites { get; set; }
        public NotifyTaskCompletion<List<Book>> Books { get; set; }
        public NotifyTaskCompletion<List<string>> Categories { get; set; }

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SearchText = "";

            // In design mode sample: IsInDesignMode
        }

        private string _searchText = null;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                if (string.IsNullOrEmpty(_searchText))
                {
                    Books = new NotifyTaskCompletion<List<Book>>(RssFeedService.GetMekFeedAsync());
                    //TODO: Categories = new NotifyTaskCompletion<List<string>>(LocalMekService.GetCategories());
                    Favorites = new NotifyTaskCompletion<List<Book>>(FavoriteService.SearchFavoriteAsync(""));
                }
                else if (_searchText == PAGE_FAVORITE)
                {
                    Books = new NotifyTaskCompletion<List<Book>>(FavoriteService.SearchFavoriteAsync(""));
                    Favorites = new NotifyTaskCompletion<List<Book>>(FavoriteService.SearchFavoriteAsync(_searchText));
                }
                else
                {
                    //Regenerate Async results
                    Books = new NotifyTaskCompletion<List<Book>>(MekService.SearchBookAsync(_searchText));
                    Favorites = new NotifyTaskCompletion<List<Book>>(FavoriteService.SearchFavoriteAsync(_searchText));
                }

            }
        }
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_searchText))
                    return "Könyvajánló";
                else if (_searchText == PAGE_FAVORITE)
                    return "Kedvencek";
                return String.Format("'{0}' keresés találatai", _searchText);
            }
        }
    }
}