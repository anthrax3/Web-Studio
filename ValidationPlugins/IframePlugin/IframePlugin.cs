using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using HtmlAgilityPack;
using IframePlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace IframePlugin
{
    /// <summary>
    ///     This plugin checks if there are iframes in your project
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Iframe")]
    [ExportMetadata("After", "Include")]
    public class IframePlugin : IValidation
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public IframePlugin()
        {
            View = new View(this);
        }

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
        public ICategoryType Type { get; } = OptimizationType.Instance;

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
            var counter = 0;
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//iframe"); //Get iframes
                if (nodes == null) continue;
                foreach (var node in nodes)
                {
                    AnalysisResults.Add(new AnalysisResult(file, node.Line, Name, Strings.IframeFound,
                        ErrorType.Instance));
                    counter++;
                }
            }
            if (counter != 0)
                AnalysisResults.Add(new AnalysisResult("", 0, Name, string.Format(Strings.Found, counter),
                    InfoType.Instance));
            return AnalysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;

            return null;
        }

        #endregion
    }
}