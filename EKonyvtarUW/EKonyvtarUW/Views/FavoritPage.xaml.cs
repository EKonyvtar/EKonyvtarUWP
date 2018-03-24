using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System.Collections.Generic;
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
        private FavoritViewModel vm;
        private List<Book> selectedBookList;

        public FavoritPage()
        {
            this.InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            vm = new FavoritViewModel(null);
            this.DataContext = vm;
        }

        private async void FavoriteBook_Click(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(BookPage), (Book)e.ClickedItem);
        }

        private void DeleteFavorite_Click(object sender, RoutedEventArgs e)
        {
            //TODO: notify observable collection change
            if (selectedBookList == null) return;
            foreach (var book in selectedBookList)
                FavoriteService.RemoveBook(book);
            Refresh();
        }

        private void FavoriteGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (edit.IsChecked ?? false)
            {
                selectedBookList = new List<Book>();
                var favoriteGridView = (GridView)sender;
                if (favoriteGridView.SelectedItems.Count > 0)
                    foreach (Book item in favoriteGridView.SelectedItems)
                        selectedBookList.Add(item);

            }
        }
    }
}
