using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Web_Studio.Converters
{
    /// <summary>
    ///  Converter if instance is null or it is instanced.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        ///    When the value is null, the visibility is hidden
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        /// <summary>
        ///  When visibility is hidden, the instance has a null value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Hidden) return null;
            return new Object();

        }
    }
}