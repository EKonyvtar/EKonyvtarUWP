
using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EKonyvtarUW.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTaskCompletion<List<Recommendation>> MekFeed { get; set; }
        public NotifyTaskCompletion<List<string>> Categories { get; set; }
        private string _helloWorld;

        public WelcomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            HelloWorld = IsInDesignMode
                ? "Runs in design mode"
                : "Runs in runtime mode";

            GotoPage1Command = new RelayCommand(() => _navigationService.NavigateTo("BrowsePage"));
            GotoPage1Command = new RelayCommand(() => _navigationService.NavigateTo("Book"));

            MekFeed = new NotifyTaskCompletion<List<Recommendation>>(RssFeedService.GetMekFeedAsync());
            Categories = new NotifyTaskCompletion<List<string>>(LocalMekService.GetCategories());
        }

        public string HelloWorld
        {
            get { return _helloWorld; }
            set { Set(() => HelloWorld, ref _helloWorld, value); }
        }

        public RelayCommand GotoPage1Command { get; private set; }
        public RelayCommand OpenBookPageCommand { get; private set; }
    }
}