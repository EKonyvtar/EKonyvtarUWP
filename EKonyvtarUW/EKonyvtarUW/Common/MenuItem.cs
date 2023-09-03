using CommunityToolkit.Mvvm.ComponentModel;
using System;


namespace EKonyvtarUW.Common
{
    public class MenuItem : ObservableObject
    {
        private string _icon;
        private Type _pageType;
        private string _title;

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public Type PageType
        {
            get { return _pageType; }
            set { SetProperty(ref _pageType, value); }
        }
    }
}