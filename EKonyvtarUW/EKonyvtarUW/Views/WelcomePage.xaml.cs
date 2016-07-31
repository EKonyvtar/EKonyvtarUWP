using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        private WelcomeViewModel vm;

        public WelcomePage()
        {
            this.InitializeComponent();
            vm = new WelcomeViewModel(null);
            this.DataContext = vm;
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
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

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            try
            {
                BookResults.Width = this.ActualWidth;
                // BookResults.Height = this.ActualHeight - 200;
            }
            catch
            {
                //Leave broken
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                vm.SearchText = e.Parameter.ToString();
            }
        }
    }
}
