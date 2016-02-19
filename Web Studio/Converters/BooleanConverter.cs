using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Web_Studio.Converters
{
    /// <summary>
    ///     Parametric boolean converter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BooleanConverter<T> : IValueConverter
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        /// <summary>
        ///     True value
        /// </summary>
        public T True { get; set; }

        /// <summary>
        ///     False value
        /// </summary>
        public T False { get; set; }

        /// <summary>
        ///     return a T type from bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && (bool) value ? True : False;
        }

        /// <summary>
        ///     Return bool from T type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T) value, True);
        }
    }
}