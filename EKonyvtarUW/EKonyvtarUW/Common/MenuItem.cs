using System;
using GalaSoft.MvvmLight;

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
            set { Set(ref _icon, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public Type PageType
        {
            get { return _pageType; }
            set { Set(ref _pageType, value); }
        }
    }
}