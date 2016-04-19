using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;

namespace EKonyvtarUW.ViewModels
{
    public class BrowseViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTaskCompletion<List<Book>> SearchResult { get; set; }

        private string _helloWorld;

        public string Category { get; set; }
        public bool IsLoading { get; set; }

        private string _searchText = null;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                if (String.IsNullOrEmpty(_searchText))
                {
                    _searchText = ""; //Do local stuff
                }
                else
                {
                    //Regenerate Async results
                    SearchResult = new NotifyTaskCompletion<List<Book>>(MekService.SearchBookAsync(_searchText));
                }
            }
        }
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_searchText))
                    return "Tallózó";
                return _searchText;
            }
        }

        public BrowseViewModel(INavigationService navigationService)
        {
            //Initializing Search/Browse
            IsLoading = true;
            SearchText = null;

            _navigationService = navigationService;
            HelloWorld = IsInDesignMode
               ? "Runs in design mode"
               : "Runs in runtime mode";
        }

        public string HelloWorld
        {
            get { return _helloWorld; }
            set { Set(() => HelloWorld, ref _helloWorld, value); }
        }
    }
}