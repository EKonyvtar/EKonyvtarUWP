
using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EKonyvtarUW.ViewModels
{
    public class FavoritViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService _navigationService;

        public NotifyTaskCompletion<List<Book>> Books { get; set; }

        public FavoritViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Books = new NotifyTaskCompletion<List<Book>>(FavoriteService.SearchFavoriteAsync(""));
        }
    }
}