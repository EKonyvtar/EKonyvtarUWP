using EKonyvtarUW.Models;
using EKonyvtarUW.Services;
using EKonyvtarUW.ViewModels;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
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

            this.DataContext = vm;
            Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView().DataRequested += BookPage_DataRequested;
        }

        private void BookPage_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            if (vm != null && !string.IsNullOrWhiteSpace(vm.book.Url))
            {
                //http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=http://murati.hu
                args.Request.Data.SetText(vm.book.ShareInfo);
                args.Request.Data.Properties.Title = Windows.ApplicationModel.Package.Current.DisplayName;
            }
            else
            {
                args.Request.FailWithDisplayText("Hiba a könyv megosztásakor.");
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter != null)
            {
                vm.book = (Book)e.Parameter;
                vm.book.CopyFrom(await MekService.GetBookByUrlId(vm.book.UrlId));
                vm.IsLoading = false;
            }
        }

        private async void ReadButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.book.PreferedMedia))
            {
                var dialog = new MessageDialog(BookViewModel.BookErrorString);
                await dialog.ShowAsync();
            }
            else
                Frame.Navigate(typeof(BookReader), vm.book);
        }

        private async void SaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.book.PreferedMedia))
            {
                var dialog = new MessageDialog(BookViewModel.BookErrorString);
                await dialog.ShowAsync();
            }
            else
                await Windows.System.Launcher.LaunchUriAsync(new Uri(vm.book.PreferedMedia));
        }

        private void MediaSelector_IsEnabledChanged(object sender, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            try
            {
                combo.SelectedIndex = 0;
                if (combo.Items.Count > 1)
                    combo.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            catch
            {
                //No formats available
            }
            vm.IsLoading = false;
        }

        private void MediaSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var combo = (ComboBox)sender;
                var item = (KeyValuePair<string, string>)combo.SelectedItem;
                vm.book.PreferedMedia = item.Value;
            }
            catch { }
        }

        private void FavoriteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            vm.book.ToggleFavorite();
        }

        private void Share_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private async void OpenMek_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(vm.book.Url));
        }

        private void RefreshButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookPage), vm.book);
        }
    }
}
