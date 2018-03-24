using EKonyvtarUW.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EKonyvtarUW.Common
{
    public class EmptyStateToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If set to True, conversion is reversed: True will become Collapsed.
        /// </summary>
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return EmptyStateToBooleanConverter.ConvertBoolean(value, this.IsReversed) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
