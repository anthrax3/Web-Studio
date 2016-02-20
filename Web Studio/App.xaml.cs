using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Web_Studio.Models;
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
            //Open with file as argument
            if (e.Args.Length > 0 && File.Exists(e.Args[0]) && Path.GetExtension(e.Args[0])==".ws")
            {
                ProjectModel.Instance.FullPath = Path.GetDirectoryName(e.Args[0]);
            }

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