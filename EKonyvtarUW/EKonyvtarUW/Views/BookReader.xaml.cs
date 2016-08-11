using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookReader : Page
    {
        private Book book;

        public BookReader()
        {
            this.InitializeComponent();
            book = new Book();
            WebView.Navigate(new Uri("about:blank"));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                book = (Book)e.Parameter;
                var MediaUri = new Uri(GetMediaUri(book.PreferedMedia));
                WebView.Navigate(MediaUri);
                WebView.NavigationCompleted += WebView_NavigationCompleted;
            }
        }

        private string GetMediaUri(string content)
        {
            if (Regex.IsMatch(content, "\\.pdf$"))
                return String.Format("https://docs.google.com/gview?url={0}&embedded=true", content);

            if (Regex.IsMatch(content, "\\.doc[x]?$"))
                return String.Format("http://view.officeapps.live.com/op/view.aspx?src={0}", content);

            return content;
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            Progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }
    }
}
