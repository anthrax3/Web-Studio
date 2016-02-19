using System.Globalization;
using System.Threading;
using System.Windows;
using Web_Studio.Properties;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     Config startup language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ResxLocalizationProvider.Instance.UpdateCultureList(GetType().Assembly.FullName, "Strings");
            LocalizeDictionary.Instance.IncludeInvariantCulture = false;

            var cultureInfo = CultureInfo.CurrentUICulture;

            LocalizeDictionary.Instance.Culture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        /// <summary>
        ///     Save data before close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}