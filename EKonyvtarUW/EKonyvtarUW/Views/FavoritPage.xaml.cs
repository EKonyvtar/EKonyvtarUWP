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
    public sealed partial class FavoritPage : Page
    {
        private HomeViewModel vm;

        public FavoritPage()
        {
            this.InitializeComponent();
            vm = new HomeViewModel(null);
            vm.SearchText = HomeViewModel.PAGE_FAVORITE;
            this.DataContext = vm;
        }

        private async void FavoriteBook_Click(object sender, ItemClickEventArgs e)
        {
            var book = (Book)e.ClickedItem;
            Frame.Navigate(typeof(BookPage), book);
        }
    }
}
