using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using HtmlCompressionPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;
using WebMarkupMin.Core.Minifiers;

namespace HtmlCompressionPlugin
{
    /// <summary>
    ///     Plugin to compress the html code
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "HtmlCompression")]
    [ExportMetadata("After", "TextRatio")]
    public class HtmlCompression : IValidation
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
            if (!IsAutoFixeable)
                analysisResults.Add(new AnalysisResult("", 0, Name, Strings.Disable, WarningType.Instance));
            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;

            var results = new List<AnalysisResult>();
            var htmlMinifier = new HtmlMinifier();
            long originalSize = 0, minifiedSize = 0;

            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);

            foreach (var file in filesToCheck)
            {
                var result = htmlMinifier.Minify(File.ReadAllText(file), true);

                var statistics = result.Statistics;
                if (statistics != null) // add statistics
                {
                    originalSize += statistics.OriginalSize;
                    minifiedSize += statistics.MinifiedSize;
                }
                File.WriteAllText(file, result.MinifiedContent); //Write the minified version
                //Process errors and warnings
                results.AddRange(
                    result.Errors.Select(
                        error => new AnalysisResult(file, error.LineNumber, Name, error.Message, ErrorType.Instance)));
                results.AddRange(
                    result.Warnings.Select(
                        warning =>
                            new AnalysisResult(file, warning.LineNumber, Name, warning.Message, ErrorType.Instance)));
            }

            results.Add(new AnalysisResult("", 0, Name,
                string.Format(Strings.Compression, (double) minifiedSize/originalSize), InfoType.Instance));

            return results;
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