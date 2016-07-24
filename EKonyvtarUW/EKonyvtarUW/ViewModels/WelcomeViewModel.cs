
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
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTaskCompletion<List<Book>> Books { get; set; }
        public NotifyTaskCompletion<List<string>> Categories { get; set; }

        public WelcomeViewModel(INavigationService navigationService)
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
                if (String.IsNullOrEmpty(_searchText))
                {
                    _searchText = "";
                    Books = new NotifyTaskCompletion<List<Book>>(RssFeedService.GetMekFeedAsync());
                    //Categories = new NotifyTaskCompletion<List<string>>(LocalMekService.GetCategories());
                }
                else
                {
                    //Regenerate Async results
                    Books = new NotifyTaskCompletion<List<Book>>(MekService.SearchBookAsync(_searchText));
                }
            }
        }
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_searchText))
                    return "Könyvajánló";
                return String.Format("'{0}' keresés találatai", _searchText);
            }
        }
    }
}