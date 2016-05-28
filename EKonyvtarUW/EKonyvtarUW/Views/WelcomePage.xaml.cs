using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();

           
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked_item = (Recommendation)e.ClickedItem;
            var target_uri = new ItemResolver(clicked_item.Link).Uri;
            var book = new Book() {
                Title = clicked_item.Title,
                UrlId = target_uri,
                Recommendation = clicked_item.Text
            }; 
            Frame.Navigate(typeof(BookPage), book);

        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            try
            {
                Recommendation.Width = this.ActualWidth;
                // Recommendation.Height = this.ActualHeight - 200;
            }
            catch
            {
                //Leave broken
            }
        }
    }
}
