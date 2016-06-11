using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookReader : Page
    {
        public Book book;
        public BookReader()
        {
            this.InitializeComponent();
            book = new Book();
            wv.Navigate(new Uri("about:blank"));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                // book = (Book)e.Parameter;
                // progress.Visibility = Windows.UI.Xaml.Visibility.Visible;
                // wv.Navigate(new Uri(book.ContentUri));
                wv.Navigate(new Uri((string)e.Parameter));
                wv.NavigationCompleted += Wv_NavigationCompleted;
            }
        }

        private void Wv_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            progress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            try
            {
                wv.Width = this.ActualWidth;
                wv.Height = this.ActualHeight;
            }
            catch
            {

            }
        }
    }
}
