using EKonyvtarUW.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.ComponentModel;

namespace EKonyvtarUW.ViewModels
{
    //Todo: fix class names for pages and views
    public class BookViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        private bool _IsLoading = true;
        public bool IsReady { get { return !IsLoading; } }
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyPropertyChanged("IsLoading");
                NotifyPropertyChanged("IsReady");

            }
        }

        private Book _book;
        public Book book
        {
            get { return _book; }
            set
            {
                _book = value;
                NotifyPropertyChanged("book");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public BookViewModel()
        {
            IsLoading = true;
            //ReadCommand = new RelayCommand(() => Frame.Navigate(typeof(BookPage), book););

            // In design mode sample
            if (IsInDesignMode)
            {
                //
            }
        }
        public BookViewModel(INavigationService navigationService) : this()
        {
            _navigationService = navigationService;
        }

        public RelayCommand ReadCommand { get; private set; }
    }
}