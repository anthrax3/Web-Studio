using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Web_Studio.Converters
{
    /// <summary>
    /// This converter checks if we can make the binding
    /// </summary>
    public class SouceToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// Only make the binding if it is an image
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as String;
            var s = Path.GetExtension(path);
            if (s != null)
            {
                var extension = s.ToLower();
                switch (extension)
                {
                    case ".png":
                    case ".jpg":
                    case ".gif":
                        return value;
                    default:
                        return null;
                }
            }
            return null;
        }

     
        /// <summary>
        ///  Not used
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //Ignore
        }
    }
}