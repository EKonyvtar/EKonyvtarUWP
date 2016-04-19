using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowsePage : Page
    {
        private BrowseViewModel vm;

        public BrowsePage()
        {
            this.InitializeComponent();
            vm = new BrowseViewModel(null);
            this.DataContext = vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                vm.SearchText = e.Parameter.ToString();
                //vm.Refresh();
            }
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var minBook = (Book)e.ClickedItem;
            //var target_uri = new ItemResolver(clicked_item.Link).Uri;
            var book = await MekService.GetBookByUid(minBook.Id);
            Frame.Navigate(typeof(BookPage), book);

        }
    }
}
