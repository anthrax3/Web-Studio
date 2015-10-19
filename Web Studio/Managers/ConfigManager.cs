using System;
using System.Configuration;
using System.IO;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
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
        public  bool EditorShowLineNumbers { get; set; }
        /// <summary>
        /// Color for links
        /// </summary>
        public  Brush EditorLinkTextForegroundBrush { get; set; }

        /// <summary>
        /// Font size in editor
        /// </summary>
        public int EditorFontSize { get; set; }

        /// <summary>
        /// Default constructor (Singleton pattern)
        /// </summary>
        private ConfigManager()
        {
            
        }
        

        /// <summary>
        /// Load config options. First try to load from settings, second use defaults values and apply changes
        /// </summary>
        public void Load(TextEditor myTextEditor)
        {
            Instance.EditorShowLineNumbers = Properties.Settings.Default.EditorShowLineNumbers;
            Instance.EditorFontSize = Properties.Settings.Default.EditorFontSize;
            Instance.EditorLinkTextForegroundBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Properties.Settings.Default.EditorLinkTextForegroundBrush));
            myTextEditor.DataContext = Instance;
            myTextEditor.TextArea.TextView.LinkTextForegroundBrush = Instance.EditorLinkTextForegroundBrush;
        }

        /// <summary>
        /// Save to Settings
        /// </summary>
        public  void Save()
        {
            Properties.Settings.Default.EditorShowLineNumbers = Instance.EditorShowLineNumbers;
            Properties.Settings.Default.EditorLinkTextForegroundBrush = Instance.EditorLinkTextForegroundBrush.ToString();
            Properties.Settings.Default.EditorFontSize = Instance.EditorFontSize;
            Properties.Settings.Default.Save();
        }
        
    }
}