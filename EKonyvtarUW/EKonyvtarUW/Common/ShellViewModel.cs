using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using EKonyvtarUW.Services;

namespace EKonyvtarUW.Common
{
    public class ShellViewModel : ViewModelBase
    {
        private bool _isSplitViewPaneOpen;
        private MenuItem _selectedMenuItem;

        public ShellViewModel()
        {
            ToggleSplitViewPaneCommand = new RelayCommand(() => IsSplitViewPaneOpen = !IsSplitViewPaneOpen);
        }

        public RelayCommand ToggleSplitViewPaneCommand { get; private set; }

        public bool IsSplitViewPaneOpen
        {
            get { return _isSplitViewPaneOpen; }
            set { Set(ref _isSplitViewPaneOpen, value); }
        }


        public async void Search(string text)
        {
            var results = await LocalMekService.SearchBookAsync(text);

        }
        public MenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                if (Set(ref _selectedMenuItem, value))
                {
                    RaisePropertyChanged(() => SelectedPageType);

                    // auto-close split view pane
                    IsSplitViewPaneOpen = false;
                }
            }
        }

        public Type SelectedPageType
        {
            get { return _selectedMenuItem?.PageType; }
            set
            {
                // select associated menu item
                SelectedMenuItem = MenuItems
                    .FirstOrDefault(m => m.PageType == value);
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; } = new ObservableCollection<MenuItem>();
    }
}