using EKonyvtarUW.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {

        public Book book;
        public AboutPage()
        {
            this.InitializeComponent();
            book = new Book();
        }

        private async void OpenMekHu_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://mek.oszk.hu"));
        }

        private async void OpenMuratiHu_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://murati.hu"));
        }

        private void AboutSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(HomePage), sender.Text);
        }
    }
}
