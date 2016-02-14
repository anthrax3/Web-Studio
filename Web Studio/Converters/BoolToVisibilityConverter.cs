using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Web_Studio.Converters
{
    /// <summary>
    /// Convert a bool to a visibility type
    /// </summary>
    public sealed class BoolToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        public BoolToVisibilityConverter(Visibility trueValue, Visibility falseValue) : base(trueValue, falseValue)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    
}
}