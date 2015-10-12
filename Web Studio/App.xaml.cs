using System.Windows;
using Web_Studio.Managers;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ConfigManager.Instance.Save();
        }
    }
}