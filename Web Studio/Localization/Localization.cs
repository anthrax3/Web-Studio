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
    public class Localization
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

        /// <summary>
        /// Search for the best localization that we have with your system language
        /// </summary>
        /// <param name="languages"></param>
        /// <returns></returns>
        public static CultureInfo GetLanguage(List<CultureInfo> languages)
        {
            var myCulture = CultureInfo.CurrentUICulture;
            if (languages.Any(c => string.Equals(c.Name, myCulture.Name, StringComparison.CurrentCultureIgnoreCase))) //we have the language
            {
                return myCulture;
            }
            var parentCulture =  languages.FirstOrDefault(c => string.Equals(c.Name, myCulture.Parent.Name, StringComparison.CurrentCultureIgnoreCase));
            if (parentCulture != null) //We have the parent example ES-ES to ES
            {
                return parentCulture;
            }
            var brotherCulture = languages.FirstOrDefault(c => string.Equals(c.Parent.Name,myCulture.Parent.Name, StringComparison.CurrentCultureIgnoreCase));
            if (brotherCulture != null) return brotherCulture; //use a language with the same parent, example ES-ES for ES-MX

            return new CultureInfo("en"); //Use English as default value
        }
    }
}