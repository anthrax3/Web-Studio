using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using NormalizeCssPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace NormalizeCssPlugin
{
    /// <summary>
    ///     This plugin adds normalize.css to all pages
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "NormalizeCss")]
    [ExportMetadata("After", "Css")]
    public class NormalizeCss : IValidation
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
        public ICategoryType Type { get; } = StyleType.Instance;

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
            var analysisResults = new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;
            if (!IsAutoFixeable)
                analysisResults.Add(new AnalysisResult("", 0, Name, Strings.Disabled, WarningType.Instance));
            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            var filesToCheck = Directory.GetFiles(Path.Combine(projectPath, "css"), "*.css", SearchOption.AllDirectories);
            foreach (var file in filesToCheck) //Add normaliza as first style in all html files
            {
                var oldContent = File.ReadAllText(file);
                var normalizeContent = Normalize.Code;
                File.WriteAllText(file, normalizeContent + " \n " + oldContent);
            }
            return new List<AnalysisResult> {new AnalysisResult("",0,Name,Strings.Added,InfoType.Instance)};
        }

        /// <summary>
        ///     View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        #endregion
    }
}