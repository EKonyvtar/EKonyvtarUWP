using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using EKonyvtarUW.Common;
using EKonyvtarUW.Views;
using CommonServiceLocator;

namespace EKonyvtarUW.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register(() => navigationService);

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                //SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }

            SimpleIoc.Default.Register<HomeViewModel>();
        }

        public HomeViewModel Welcome => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public BookViewModel Book => ServiceLocator.Current.GetInstance<BookViewModel>();

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationServiceUwp();
            navigationService.Configure("Home", typeof(HomePage));
            navigationService.Configure("Favorit", typeof(FavoritPage));
            navigationService.Configure("Book", typeof(BookPage));

            return navigationService;
        }
    }
}