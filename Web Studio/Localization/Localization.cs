using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web_Studio.Events;
using Web_Studio.Properties;
using WPFLocalizeExtension.Engine;

namespace Web_Studio.Localization
{
    /// <summary>
    /// Class to manage the app language
    /// </summary>
    public static class Localization
    {
        /// <summary>
        /// It changes the language with the culture provided
        /// </summary>
        /// <param name="culture"></param>
        public static void ChangeLanguage(CultureInfo culture)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;    //Other threads
            CultureInfo.DefaultThreadCurrentUICulture = culture;  // Other threads
            EventSystem.Publish(new ChangedLanguageEvent {CultureInfo = culture});
            Settings.Default.Language = culture.Name;
        } 
    }
}