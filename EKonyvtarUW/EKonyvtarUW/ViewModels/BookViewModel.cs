using EKonyvtarUW.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace EKonyvtarUW.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public bool IsLoading { get; set; }

        public Book book { get; set; }

        public BookViewModel()
        {
            //IsInDesignMode
            IsLoading = true;
            GoBackCommand = new RelayCommand(() => _navigationService.GoBack());
        }
        public BookViewModel(INavigationService navigationService) : this()
        {
            _navigationService = navigationService;
        }

        public RelayCommand GoBackCommand { get; private set; }
    }
}