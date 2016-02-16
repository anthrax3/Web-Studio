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
        public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    
}
}