using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Views;

namespace EKonyvtarUW.Common
{
    /// <summary>
    ///     Windows 8 and Windows Phone Application 8.1 implementation of
    ///     <see cref="T:GalaSoft.MvvmLight.Views.INavigationService" />.
    /// </summary>
    public class NavigationServiceUwp : INavigationService
    {
        /// <summary>
        ///     The key that is returned by the <see cref="P:GalaSoft.MvvmLight.Views.NavigationService.CurrentPageKey" /> property
        ///     when the current Page is the root page.
        /// </summary>
        public const string RootPageKey = "-- ROOT --";

        /// <summary>
        ///     The key that is returned by the <see cref="P:GalaSoft.MvvmLight.Views.NavigationService.CurrentPageKey" /> property
        ///     when the current Page is not found.
        ///     This can be the case when the navigation wasn't managed by this NavigationService,
        ///     for example when it is directly triggered in the code behind, and the
        ///     NavigationService was not configured for this page type.
        /// </summary>
        public const string UnknownPageKey = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        /// <summary>
        ///     The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                var lockTaken = false;
                Dictionary<string, Type> dictionary = null;
                try
                {
                    Monitor.Enter(dictionary = _pagesByKey, ref lockTaken);
                    var frame = ((Shell)Window.Current.Content).RootFrame;
                    if (frame.BackStackDepth == 0)
                        return RootPageKey;
                    if (frame.Content == null)
                        return UnknownPageKey;
                    var currentType = frame.Content.GetType();
                    if (_pagesByKey.All(p => p.Value != currentType))
                        return UnknownPageKey;
                    return _pagesByKey.FirstOrDefault(i => i.Value == currentType).Key;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(dictionary);
                }
            }
        }

        /// <summary>
        ///     If possible, discards the current page and displays the previous page
        ///     on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            var frame = ((Shell)Window.Current.Content).RootFrame;
            if (!frame.CanGoBack)
                return;
            frame.GoBack();
        }

        /// <summary>
        ///     Displays a new page corresponding to the given key.
        ///     Make sure to call the
        ///     <see cref="M:GalaSoft.MvvmLight.Views.NavigationService.Configure(System.String,System.Type)" />
        ///     method first.
        /// </summary>
        /// <param name="pageKey">
        ///     The key corresponding to the page
        ///     that should be displayed.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///     When this method is called for
        ///     a key that has not been configured earlier.
        /// </exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        ///     Displays a new page corresponding to the given key,
        ///     and passes a parameter to the new page.
        ///     Make sure to call the
        ///     <see cref="M:GalaSoft.MvvmLight.Views.NavigationService.Configure(System.String,System.Type)" />
        ///     method first.
        /// </summary>
        /// <param name="pageKey">
        ///     The key corresponding to the page
        ///     that should be displayed.
        /// </param>
        /// <param name="parameter">
        ///     The parameter that should be passed
        ///     to the new page.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        ///     When this method is called for
        ///     a key that has not been configured earlier.
        /// </exception>
        public virtual void NavigateTo(string pageKey, object parameter)
        {
            var lockTaken = false;
            Dictionary<string, Type> dictionary = null;
            try
            {
                Monitor.Enter(dictionary = _pagesByKey, ref lockTaken);
                if (!_pagesByKey.ContainsKey(pageKey))
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                        nameof(pageKey));
                ((Shell)Window.Current.Content).RootFrame.Navigate(_pagesByKey[pageKey], parameter);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(dictionary);
            }
        }

        /// <summary>
        ///     Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">
        ///     The key that will be used later
        ///     in the <see cref="M:GalaSoft.MvvmLight.Views.NavigationService.NavigateTo(System.String)" /> or
        ///     <see cref="M:GalaSoft.MvvmLight.Views.NavigationService.NavigateTo(System.String,System.Object)" /> methods.
        /// </param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public void Configure(string key, Type pageType)
        {
            var lockTaken = false;
            Dictionary<string, Type> dictionary = null;
            try
            {
                Monitor.Enter(dictionary = _pagesByKey, ref lockTaken);
                if (_pagesByKey.ContainsKey(key))
                    throw new ArgumentException("This key is already used: " + key);
                if (_pagesByKey.Any(p => p.Value == pageType))
                    throw new ArgumentException("This type is already configured with key " +
                                                _pagesByKey.First(p => p.Value == pageType).Key);
                _pagesByKey.Add(key, pageType);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(dictionary);
            }
        }
    }
}