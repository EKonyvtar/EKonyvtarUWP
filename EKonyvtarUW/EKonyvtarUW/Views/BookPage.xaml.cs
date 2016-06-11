using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookPage : Page
    {
        private BookViewModel vm;
        public BookPage()
        {
            this.InitializeComponent();
            vm = new BookViewModel();
            //http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=http://murati.hu
            this.DataContext = vm;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //target_item.Uri
            if (e != null && e.Parameter != null)
            {
                vm.book = (Book)e.Parameter;
                vm.book.CopyFrom(await MekService.GetBookByUrlId(vm.book.UrlId));
                vm.IsLoading = false;
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookReader), vm.book.ContentUri);
        }

        private void ComboBox_IsEnabledChanged(object sender, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            try
            {
                combo.SelectedIndex = 0;
            }
            catch {
                //No formats available
            }
            vm.IsLoading = false;
        }

        private async void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            //StorageFile file = picker.PickSaveFileAndContinue();
        }
    }
}
