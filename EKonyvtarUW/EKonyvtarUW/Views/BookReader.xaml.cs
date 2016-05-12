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
    public sealed partial class BookReader : Page
    {
        //private BookViewModel vm;
        public BookReader()
        {
            this.InitializeComponent();
            //vm = new BookViewModel();
            //this.DataContext = vm;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //target_item.Uri
            if (e != null && e.Parameter != null)
            {
                //vm.book = (Book)e.Parameter;
                //vm.IsLoading = false;
            }
        }
    }
}
