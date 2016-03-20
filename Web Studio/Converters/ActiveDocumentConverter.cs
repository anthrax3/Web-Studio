using System;
using System.Windows.Data;
using Web_Studio.Editor;

namespace Web_Studio.Converters
{
    /// <summary>
    ///  Converter to get the active document
    /// </summary>
    public class ActiveDocumentConverter : IValueConverter
    {
        /// <summary>
        /// it only changes if the value is an editor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is EditorViewModel)
                return value;

            return Binding.DoNothing;
        }

        /// <summary>
        ///  it only changes if the value is an editor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is EditorViewModel)
                return value;

            return Binding.DoNothing;
        }

    }
}