using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using HtaccessPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace HtaccessPlugin
{
    /// <summary>
    ///     Plugin to generate the .htaccess file
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Htaccess")]
    [ExportMetadata("After", "Links")]
    public class HtaccessPlugin : IValidation
    {
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix; 

        #region IValidation

        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public string Name => Strings.Name;

        /// <summary>
        ///     Description
        /// </summary>
        public string Description => Strings.Description;

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        public ICategoryType Type { get; } = DevelopmentType.Instance;
        
        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        public bool IsAutoFixeable { get; set; } = false;

        /// <summary>
        ///     Is enabled this plugin
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        ///     Method to validate the project with this plugin
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public List<AnalysisResult> Check(string projectPath)
        {
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;

            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            if (!File.Exists(htaccessPath))
            {
                analysisResults.Add(new AnalysisResult
                {
                    File = "",
                    Line = 0,
                    PluginName = Name,
                    Type = ErrorType.Instance,
                    Message = Strings.NotFound
                });
            }

            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            var source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".htaccess");
            File.Copy(source, htaccessPath); //Copy a well format .htaccess with all optimizations
            List<AnalysisResult> list = new List<AnalysisResult> {HtaccessGenerated(htaccessPath)};
            return list; 
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        /// <summary>
        /// Creates the generated message
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalysisResult HtaccessGenerated(string file)
        {
            return new AnalysisResult
            {
                File = file,
                Line = 0,
                PluginName = Name,
                Type = InfoType.Instance,
                Message = Strings.Generated
            };
        }

        #endregion
    }
}