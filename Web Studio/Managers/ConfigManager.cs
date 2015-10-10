using System;
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;
using Web_Studio.Utils;

namespace Web_Studio.Managers
{
    /// <summary>
    /// Class to manage the Web Studio settings
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// Singleton pattern
        /// </summary>
        [JsonIgnore]
        public static ConfigManager Instance { get; set; } = new ConfigManager();
        /// <summary>
        /// Enable to show line numbers in editor
        /// </summary>
        public  bool EditorShowLineNumbers { get; set; } = true;
        /// <summary>
        /// Color for links
        /// </summary>
        public  Brush EditorLinkTextForegroundBrush { get; set; } = Brushes.DeepSkyBlue;
        /// <summary>
        /// Font size in editor
        /// </summary>
        public  int EditorFontSize { get; set; } = 55;

        /// <summary>
        /// Default constructor (Singleton pattern)
        /// </summary>
        private ConfigManager()
        {
            
        }

        /// <summary>
        /// Load config options. First try to load from config file, second use defaults values
        /// </summary>
        public void Load()
        {
            try
            {
                //Try to find the config file
                using (StreamReader file = File.OpenText(@"config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Instance = (ConfigManager) serializer.Deserialize(file, typeof (ConfigManager));
                }
            }
            catch (Exception) //FileException or JSonException --> use default values
            {
                // ignored
            }
            
        }

        /// <summary>
        /// Save instance to file
        /// </summary>
        public  void Save()
        {
            if (Json.ObjectToFile(Instance, "config.json"))
            {
                //error when saving
            }
        }
        
    }
}