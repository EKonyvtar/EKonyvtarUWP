using EKonyvtarUW.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EKonyvtarUW.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {

        public Book book;
        public Page3()
        {
            this.InitializeComponent();
            book = new Book();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
           
        }
    }
}
