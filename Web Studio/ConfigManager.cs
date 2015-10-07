using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Navigation;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json;

namespace Web_Studio
{
    
    public class ConfigManager
    {
        [JsonIgnore]
        public static ConfigManager Instance { get; set; } = new ConfigManager();
        [JsonIgnore]
        public TextEditor MyTextEditor { get; set; }
        public  bool EditorShowLineNumbers { get; set; } = true;
        public  Brush EditorLinkTextForegroundBrush { get; set; } = Brushes.DeepSkyBlue;
        public  int EditorFontSize { get; set; } = 55;

        private ConfigManager()
        {
            
        }

        /// <summary>
        /// Load config options. First try to load from config file, second use defaults values
        /// </summary>
        /// <param name="myTextEditor"></param>
        public void Load(TextEditor myTextEditor)
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
            catch (Exception) //FileException or JSonException --> Load default values
            {
                // ignored
            }
            finally
            {
                Instance.MyTextEditor = myTextEditor;
                Instance.MyTextEditor.ShowLineNumbers = Instance.EditorShowLineNumbers;
                Instance.MyTextEditor.TextArea.TextView.LinkTextForegroundBrush = Instance.EditorLinkTextForegroundBrush;
                Instance.MyTextEditor.FontSize = Instance.EditorFontSize;
            }

        }

        public  void Save()
        {
            using (StreamWriter file = File.CreateText(@"config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, Instance);
                }
        }
        
    }
}