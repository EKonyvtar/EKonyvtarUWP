using System.Linq;
using Windows.UI.Xaml.Controls;
using EKonyvtarUW.Common;
using EKonyvtarUW.Views;
using EKonyvtarUW.Services;
using System.Threading.Tasks;

namespace EKonyvtarUW
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            InitializeComponent();

            var vm = new ShellViewModel();
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Ajánló", PageType = typeof(WelcomePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Böngésző", PageType = typeof(BrowsePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Kedvencek", PageType = typeof(BookPage) });
            //vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Page 3", PageType = typeof(Page3) });

            // select the first menu item
            vm.SelectedMenuItem = vm.MenuItems.First();

            ViewModel = vm;
        }

        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame => Frame;

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(BrowsePage), sender.Text);
        }
    }
}