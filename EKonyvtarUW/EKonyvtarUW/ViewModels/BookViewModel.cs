using EKonyvtarUW.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace EKonyvtarUW.ViewModels
{
    //Todo: fix class names for pages and views
    public class BookViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public bool IsLoading { get; set; }

        public Book book { get; set; }

        public BookViewModel()
        {
            IsLoading = true;
            GoBackCommand = new RelayCommand(() => _navigationService.GoBack());

            // In design mode sample
            if (IsInDesignMode)
            {
                IsLoading = false;
                book = new Book()
                {
                    Title = "Teszt könyv",
                    Creators = "Akos Murati",
                    Summary = "Multiple \n Line \n Samples"
                };
            }            
        }
        public BookViewModel(INavigationService navigationService) : this()
        {
            _navigationService = navigationService;
        }

        public RelayCommand GoBackCommand { get; private set; }
    }
}