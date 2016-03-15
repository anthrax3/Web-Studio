using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Mvvm;
using ValidationInterface;
using Web_Studio.PluginManager;
using FastObservableCollection;
using Prism.Commands;

namespace Web_Studio.ViewModels
{
    /// <summary>
    /// View model of ValidationPlugins view
    /// </summary>
    public class ValidationPluginsViewModel : BindableBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ValidationPluginsViewModel()
        {
        
            foreach (Lazy<IValidation, IValidationMetadata> plugin in ValidationPluginManager.Plugins)
            {
                Plugins.Add(plugin.Value);
            }
            CollectionViewSource.GetDefaultView(Plugins).GroupDescriptions.Add(new PropertyGroupDescription("Type"));  //Grouping
            ConfigurationCommand = new DelegateCommand<IValidation>(Configuration);
        

        }


        /// <summary>
        /// Load the configuration UI of the selected plugin
        /// </summary>
        /// <param name="validation"></param>
        private void Configuration(IValidation validation)
        {
           //ConfigurationUI = validation.View;
          
        }

        /// <summary>
        /// Button configuration command
        /// </summary>
        public DelegateCommand<IValidation> ConfigurationCommand { get; private set; }

        /// <summary>
        /// Validation plugins
        /// </summary>
        public ObservableCollection<IValidation> Plugins { get; set; }  = new ObservableCollection<IValidation>();


        private UserControl _configurationUI;
        /// <summary>
        /// Configuration user interface of the plugin
        /// </summary>
        public UserControl ConfigurationUI
        {
            get { return _configurationUI; }
            set { SetProperty(ref _configurationUI, value); }
        }
       
    }
}