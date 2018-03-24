using EKonyvtarUW.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EKonyvtarUW.Common
{
    public class EmptyStateToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// If set to True, conversion is reversed: True will become Collapsed.
        /// </summary>
        public bool IsReversed { get; set; }

        public static bool ConvertBoolean(object value, bool IsReversed = false)
        {
            var returnValue = (value != null);

            if (value != null)
            {
                try // to get meaningful results from various types
                {
                    Type valueType = value.GetType();
                    if (valueType == typeof(List<Book>))
                        returnValue = (((List<Book>)value).Count > 0);

                    else if (valueType == typeof(string))
                        returnValue = !string.IsNullOrWhiteSpace((string)value);
                }
                catch { }
            }

            if (IsReversed) returnValue = !returnValue;
            return returnValue;
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConvertBoolean(value, this.IsReversed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
