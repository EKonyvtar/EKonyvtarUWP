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
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Könyvajánló", PageType = typeof(WelcomePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Névjegy", PageType = typeof(AboutPage) });

            // select the first menu item
            vm.SelectedMenuItem = vm.MenuItems.First();

            ViewModel = vm;
        }

        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame => Frame;

        private async void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Frame.Navigate(typeof(WelcomePage), sender.Text);
        }
    }
}