using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using MS.WindowsAPICodePack.Internal;
using Web_Studio.Models.Project;
using Web_Studio.Properties;
using Web_Studio.Utils;
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
            Telemetry.Telemetry.Initialize();

            if (!string.IsNullOrEmpty(Settings.Default.Language))
            {
                Localization.Localization.ChangeLanguage(new CultureInfo(Settings.Default.Language));
            }
            else
            {
                var cultureInfo = CultureInfo.CurrentCulture;
                Localization.Localization.ChangeLanguage(cultureInfo);
            }
         
        
            TryCreateShortcut();

            //Open with file as argument
            if (e.Args.Length > 0 && File.Exists(e.Args[0]) && Path.GetExtension(e.Args[0]) == ".ws")
            {
                ProjectModel.Open(e.Args[0]);
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
            ProjectModel.Instance.Save();
            Telemetry.Telemetry.TelemetryClient.Flush();
        }

        /// <summary>
        /// Detects program shortcut (we need it for notifications)
        /// </summary>
        /// <returns></returns>
        private bool TryCreateShortcut()
        {
            String shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Web Studio.lnk";
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(shortcutPath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates the shortcut
        /// </summary>
        /// <param name="shortcutPath"></param>
        private void InstallShortcut(String shortcutPath)
        {
            // Find the path to the current executable
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            ShellHelpers.IShellLinkW newShortcut = (ShellHelpers.IShellLinkW)new ShellHelpers.CShellLink();

            // Create a shortcut to the exe
            ShellHelpers.ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
            ShellHelpers.ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property
            ShellHelpers.IPropertyStore newShortcutProperties = (ShellHelpers.IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(Web_Studio.Properties.Resources.AppId))
            {
                ShellHelpers.ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk
            ShellHelpers.IPersistFile newShortcutSave = (ShellHelpers.IPersistFile)newShortcut;

            ShellHelpers.ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }


    }
}