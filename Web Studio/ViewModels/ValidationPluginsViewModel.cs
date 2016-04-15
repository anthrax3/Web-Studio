using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private static ObservableCollection<Lazy<IValidation, IValidationMetadata>> _plugins;
        private Lazy<IValidation,IValidationMetadata> _pluginSelected;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ValidationPluginsViewModel()
        { 
            Plugins = ValidationPluginManager.Plugins;
            if (Plugins != null)
            {    //Grouping
                CollectionViewSource.GetDefaultView(Plugins)
                    .GroupDescriptions.Add(new PropertyGroupDescription("Value.Type"));
            }
               
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
        public static ObservableCollection<Lazy<IValidation, IValidationMetadata>> Plugins {
            get { return _plugins; }
            set
            {
                _plugins = value;
                NotifyStaticPropertyChanged("Plugins"); 
            }
        } 

        /// <summary>
        ///     Configuration user interface of the plugin
        /// </summary>
        public static  UserControl ConfigurationUI     
        {
            get { return _configurationUI; }
            set
            {
                _configurationUI = value;
                NotifyStaticPropertyChanged("ConfigurationUI");
            }
        }


        /// <summary>
        ///     Load the configuration UI of the selected plugin
        /// </summary>
        /// <param name="validation"></param>
        private void Configuration(Lazy<IValidation, IValidationMetadata> validation)
        {
            ConfigurationUI = validation.Value.GetView();
        }

        /// <summary>
        /// Property changed static implementation
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            if (StaticPropertyChanged != null)
                StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}