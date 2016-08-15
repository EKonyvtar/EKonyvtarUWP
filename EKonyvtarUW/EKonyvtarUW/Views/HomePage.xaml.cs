using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private HomeViewModel vm;

        public HomePage()
        {
            this.InitializeComponent();
            vm = new HomeViewModel(null);
            this.DataContext = vm;
        }

        private async void Book_Click(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(BookPage), (Book)e.ClickedItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                vm = new HomeViewModel(null) {
                   SearchText = e.Parameter.ToString()
                };
                this.DataContext = vm;
            }
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(HomePage), sender.Text);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomePage), "");
        }
    }
}
