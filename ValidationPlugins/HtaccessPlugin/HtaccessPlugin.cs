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
    [ExportMetadata("After", "")]
    public class HtaccessPlugin : IValidation
    {
        /// <summary>
        ///     Default constructor, inject the view
        /// </summary>
        public HtaccessPlugin()
        {
            View = new View(this);
        }

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix; //Todo

        #region IValidation

        /// <summary>
        ///     View showed when you select the plugin
        /// </summary>
        public UserControl View { get; }

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
        ///     Results of the check method.
        /// </summary>
        public List<AnalysisResult> AnalysisResults { get; } = new List<AnalysisResult>();

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
            AnalysisResults.Clear();
            if (!IsEnabled) return AnalysisResults;

            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            if (!File.Exists(htaccessPath))
            {
                AnalysisResults.Add(new AnalysisResult
                {
                    File = "",
                    Line = 0,
                    PluginName = Name,
                    Type = ErrorType.Instance,
                    Message = Strings.NotFound
                });
            }

            return AnalysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public void Fix(string projectPath)
        {
            if (!IsAutoFixeable) return;
            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            var source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".htaccess");
            File.Copy(source, htaccessPath); //Copy a well format .htaccess with all optimizations
        }

        #endregion
    }
}