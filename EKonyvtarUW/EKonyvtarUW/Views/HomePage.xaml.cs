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
            var clicked_item = (Book)e.ClickedItem;
            var target_uri = new ItemResolver(clicked_item.UrlId).Uri;
            var book = new Book()
            {
                Title = clicked_item.Title,
                UrlId = target_uri,
                Recommendation = clicked_item.Recommendation
            };
            Frame.Navigate(typeof(BookPage), book);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                vm.SearchText = e.Parameter.ToString();
            }
        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(HomePage), sender.Text);
        }
    }
}
