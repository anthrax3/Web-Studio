using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using HtmlAgilityPack;
using ImageScalingPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace ImageScalingPlugin
{
    /// <summary>
    ///     Class to check if we use image scaling from HTML
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "ImageScaling")]
    [ExportMetadata("After", "Include")]
    public class ImageScaling : IValidation
    {
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
        public ICategoryType Type { get; } = OptimizationType.Instance;

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
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//img[@width] | //img[@height]");
                if (nodes == null) continue;
                analysisResults.AddRange(
                    nodes.Select(
                        htmlNode =>
                            new AnalysisResult(file, htmlNode.Line, Name, Strings.ScalingFound, ErrorType.Instance)));
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

            return null;
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