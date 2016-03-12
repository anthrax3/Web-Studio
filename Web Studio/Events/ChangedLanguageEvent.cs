using System.Globalization;

namespace Web_Studio.Events
{
    /// <summary>
    /// Inform that the language has changed
    /// </summary>
    public class ChangedLanguageEvent
    {
        /// <summary>
        /// New culture
        /// </summary>
        public CultureInfo CultureInfo { get; set; }
    }
}