using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Mvvm;
using ValidationInterface;
using Web_Studio.PluginManager;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     View model of ValidationPlugins view
    /// </summary>
    public class ValidationPluginsViewModel : BindableBase
    {
        private UserControl _configurationUI;

        private IValidation _pluginSelected;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ValidationPluginsViewModel()
        {
            foreach (var plugin in ValidationPluginManager.Plugins)
            {
                Plugins.Add(plugin.Value);
            }
            CollectionViewSource.GetDefaultView(Plugins).GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                //Grouping   
        }

        /// <summary>
        ///     Selected plugin in the list
        /// </summary>
        public IValidation PluginSelected
        {
            get { return _pluginSelected; }
            set
            {
                SetProperty(ref _pluginSelected, value);
                if (value != null) Configuration(value);
            }
        }

        /// <summary>
        ///     Validation plugins
        /// </summary>
        public ObservableCollection<IValidation> Plugins { get; set; } = new ObservableCollection<IValidation>();

        /// <summary>
        ///     Configuration user interface of the plugin
        /// </summary>
        public UserControl ConfigurationUI
        {
            get { return _configurationUI; }
            set { SetProperty(ref _configurationUI, value); }
        }


        /// <summary>
        ///     Load the configuration UI of the selected plugin
        /// </summary>
        /// <param name="validation"></param>
        private void Configuration(IValidation validation)
        {
            ConfigurationUI = validation.View;
        }
    }
}