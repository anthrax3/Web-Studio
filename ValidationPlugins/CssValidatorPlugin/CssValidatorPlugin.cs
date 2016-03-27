using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using CssValidatorPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;
using WebMarkupMin.MsAjax.Minifiers;

namespace CssValidatorPlugin
{
    /// <summary>
    ///     Class to validate local css files
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "CssValidator")]
    [ExportMetadata("After", "Include")]
    public class CssValidatorPlugin : IValidation
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public CssValidatorPlugin()
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
            var filesToCheck = Directory.GetFiles(projectPath, "*.css", SearchOption.AllDirectories);
            var minifier = new MsAjaxCssMinifier();
            foreach (var file in filesToCheck)
            {
                var content = File.ReadAllText(file);
                var minifyResult = minifier.Minify(content, false);
                //Errors
                AnalysisResults.AddRange(
                    minifyResult.Errors.Select(
                        error =>
                            new AnalysisResult(file.Replace("release", "src"), error.LineNumber, Strings.Name,
                                error.Message, ErrorType.Instance)));
                AnalysisResults.AddRange(
                    minifyResult.Warnings.Select(
                        warning =>
                            new AnalysisResult(file.Replace("release", "src"), warning.LineNumber, Strings.Name,
                                warning.Message, ErrorType.Instance)));
            }

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