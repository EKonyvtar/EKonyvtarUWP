using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using EKonyvtarUW.Common;
using EKonyvtarUW.Views;

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

            SimpleIoc.Default.Register<WelcomeViewModel>();
            SimpleIoc.Default.Register<BrowseViewModel>();
        }

        public WelcomeViewModel Welcome => ServiceLocator.Current.GetInstance<WelcomeViewModel>();
        public BrowseViewModel Browse => ServiceLocator.Current.GetInstance<BrowseViewModel>();
        public BookViewModel Book => ServiceLocator.Current.GetInstance<BookViewModel>();

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationServiceUwp();
            navigationService.Configure("Browse", typeof(BrowsePage));
            navigationService.Configure("Book", typeof(BookPage));

            return navigationService;
        }
    }
}