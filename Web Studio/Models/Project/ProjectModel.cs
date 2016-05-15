using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using BusyControl.Annotations;
using FtpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ValidationInterface;
using Web_Studio.Models.PluginManager;
using Web_Studio.Utils;

namespace Web_Studio.Models.Project
{
    /// <summary>
    ///     Class to manage settings of one project
    /// </summary>
    public class ProjectModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Project full path
        /// </summary>
        [JsonIgnore] private string _fullPath;

        /// <summary>
        ///     Default constructor (Singleton pattern)
        /// </summary>
        private ProjectModel()
        {
            //Do nothing
        }

        /// <summary>
        ///     Singleton pattern
        /// </summary>
        [JsonIgnore]
        public static ProjectModel Instance { get; set; } = new ProjectModel();

        /// <summary>
        ///     Full path of project folder
        /// </summary>
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                OnPropertyChanged();
            }
        }
          
        /// <summary>
        ///     Name of project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Create a new project
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            try
            {
                Directory.CreateDirectory(Instance.FullPath);
                //Create project config
                Json.ObjectToFile(Instance, Path.Combine(Instance.FullPath, Instance.Name + ".ws"));


                //Create source folder
                var srcPath = Path.Combine(Instance.FullPath, "src");
                Directory.CreateDirectory(srcPath);

                //Create js folder
                Directory.CreateDirectory(Path.Combine(srcPath, "js"));

                //Create css folder
                Directory.CreateDirectory(Path.Combine(srcPath, "css"));

                //Create font folder
                Directory.CreateDirectory(Path.Combine(srcPath, "font"));

                //Create img folder
                Directory.CreateDirectory(Path.Combine(srcPath, "img"));

                //Create index file
                File.WriteAllText(Path.Combine(srcPath, "index.html"),Templates.Html5WithIncludeTemplate());

                //Create a footer
                File.WriteAllText(Path.Combine(srcPath, "footer.html"),Templates.FooterTemplate());

                return true;
            }
            catch (Exception e)
            {
                Telemetry.Telemetry.TelemetryClient.TrackException(e);
                return false;
            }
        }

        /// <summary>
        /// Clear all information of the project and get a new project
        /// </summary>
        public void Clear()
        {
            Instance.Name = null;
            Instance.FullPath = null;
            ViewModel.Sites.Clear();
        }

        /// <summary>
        /// Save all project configuration
        /// </summary>
        public void Save()
        {
            if (FullPath != null)
            {
                //Create custom serializer
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new SetPropertiesResolver(),
                    Formatting = Formatting.Indented
                };

                JObject jsonJObject = new JObject();

                jsonJObject["Name"] = Name;
                jsonJObject["FullPath"] = FullPath;
                
                //Add all plugins to configuration file
                foreach (Lazy<IValidation, IValidationMetadata> plugin in ValidationPluginManager.Plugins)
                {
                    //Add an entry with the plugin name and its writable properties
                    jsonJObject[plugin.Metadata.Name] = JObject.FromObject(plugin.Value, JsonSerializer.Create(settings));  
                }

                jsonJObject["Sites"] = JArray.FromObject(ViewModel.Sites);
               
                File.WriteAllText(Path.Combine(Instance.FullPath, Instance.Name + ".ws"),jsonJObject.ToString(Formatting.Indented));

            }
        }

        /// <summary>
        ///     Open a project, load project config and enable project UI
        /// </summary>
        /// <param name="path"></param>
        public static void Open(string path)
        { 
            
            JObject jsonObject = JObject.Parse(File.ReadAllText(path));
            Instance.FullPath = jsonObject["FullPath"]?.ToString();
            Instance.Name = jsonObject["Name"]?.ToString();
            var result = (ObservableCollection<Site>)jsonObject["Sites"]?.ToObject(new ObservableCollection<Site>().GetType());
            if (result != null)
                ViewModel.Sites = result;
            for (int index = 0; index < ValidationPluginManager.Plugins.Count; index++)
            {
                Lazy<IValidation, IValidationMetadata> plugin = ValidationPluginManager.Plugins[index];
                var pluginJObject = jsonObject[plugin.Metadata.Name];

                if (pluginJObject != null)
                {
                    //Updata plugin configuration with the project values
                    var lazyPlugin = new Lazy<IValidation, IValidationMetadata>(() => (IValidation)pluginJObject.ToObject(plugin.Value.GetType()), plugin.Metadata);  
                    ValidationPluginManager.Plugins[index] = lazyPlugin;
                }
            }
         
            Instance.FullPath = Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Event for binding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}