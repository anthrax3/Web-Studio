using System.Globalization;
using System.Windows;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para Options.xaml
    /// </summary>
    public partial class Options
    {
        public Options()
        {
            InitializeComponent();
            this.DataContext = ConfigManager.Instance;
            ComboBoxLanguage.ItemsSource = ResxLocalizationProvider.Instance.AvailableCultures;
            //CultureInfo.CurrentUICulture.DisplayName
            ComboBoxLanguage.DisplayMemberPath = "DisplayName";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo("es");
        }
    }
}