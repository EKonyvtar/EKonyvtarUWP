using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System;
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

        private void ReadButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookReader), vm.book.ContentUri);
        }

        private void FileType_IsEnabledChanged(object sender, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            try
            {
                combo.SelectedIndex = 0;
            }
            catch
            {
                //No formats available
            }
            vm.IsLoading = false;
        }

        private async void SaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //var picker = new FileSavePicker();
            //StorageFile file = picker.PickSaveFileAndContinue();
            await Windows.System.Launcher.LaunchUriAsync(new Uri(vm.ActiveUrl));
        }

        private void FileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var combo = (ComboBox)sender;
                if (combo.Items.Count > 2)
                    combo.Visibility = Windows.UI.Xaml.Visibility.Visible;

                var item = (KeyValuePair<string, string>)combo.SelectedItem;
                vm.ActiveUrl = item.Value;
            }
            catch { }
        }

        private void FavoriteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            vm.book.ToggleFavorite();
        }

        private void Share_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Share
        }

        private async void OpenMek_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(vm.book.Url));
        }
    }
}
