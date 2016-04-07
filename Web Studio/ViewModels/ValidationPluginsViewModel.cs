using System;
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
        private static UserControl _configurationUI;

        private Lazy<IValidation,IValidationMetadata> _pluginSelected;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ValidationPluginsViewModel()
        { 
            CollectionViewSource.GetDefaultView(Plugins).GroupDescriptions.Add(new PropertyGroupDescription("Value.Type"));
                //Grouping   
        }

        /// <summary>
        ///     Selected plugin in the list
        /// </summary>
        public Lazy<IValidation, IValidationMetadata> PluginSelected
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
        public ObservableCollection<Lazy<IValidation,IValidationMetadata>> Plugins => ValidationPluginManager.Plugins;

        /// <summary>
        ///     Configuration user interface of the plugin
        /// </summary>
        public  UserControl ConfigurationUI     //TODO: static
        {
            get { return _configurationUI; }
            set { SetProperty(ref _configurationUI, value); }
        }


        /// <summary>
        ///     Load the configuration UI of the selected plugin
        /// </summary>
        /// <param name="validation"></param>
        private void Configuration(Lazy<IValidation, IValidationMetadata> validation)
        {
            ConfigurationUI = validation.Value.GetView();
        }
    }
}