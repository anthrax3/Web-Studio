using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using CssPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace CssPlugin
{
    /// <summary>
    ///     Class to manage the css in the html files
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Css")]
    [ExportMetadata("After", "JoinAndMinifyCss")]
    public class CssPlugin : IValidation
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public CssPlugin()
        {
            View = new View(this);
        }

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

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
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                CheckOrder(document, file);
                CheckInlineStyle(document, file);
            }
            return AnalysisResults;
        }

        /// <summary>
        ///     Checks if there is one use of the style attribute
        /// </summary>
        /// <param name="document"></param>
        /// <param name="file"></param>
        private void CheckInlineStyle(HtmlDocument document, string file)
        {
            var styleNodes = document.DocumentNode.SelectNodes(@"//@style");
            if (styleNodes != null)
            {
                foreach (var node in styleNodes)
                {
                    AnalysisResults.Add(new AnalysisResult(file, node.Line, Name, Strings.StyleAtt, WarningType.Instance));
                }
            }
        }


        /// <summary>
        ///     Checks if there is one js file before a css file
        /// </summary>
        /// <param name="document"></param>
        /// <param name="file"></param>
        private void CheckOrder(HtmlDocument document, string file)
        {
            var cssNodes = document.DocumentNode.SelectNodes(@"//link[@rel='stylesheet']");
            if (cssNodes == null) return;
            var cssLastLine = cssNodes.Max(t => t.Line);
            var jsNodes = document.DocumentNode.SelectNodes(@"//script");
            if (jsNodes == null) return;
            var jsFirstLine = jsNodes.Min(t => t.Line);
            if (jsFirstLine < cssLastLine)
            {
                AnalysisResults.Add(new AnalysisResult(file, jsFirstLine, Name, Strings.JsBeforeCss, ErrorType.Instance));
            }
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            var counter = 0;
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var cssNodes = document.DocumentNode.SelectNodes(@"//link[@rel='stylesheet']");
                if (cssNodes == null) continue;
                var cssNode = cssNodes.OrderByDescending(t => t.Line).FirstOrDefault();
                var jsNodes = document.DocumentNode.SelectNodes(@"//script");
                if (jsNodes == null) continue;
                var jsNodesBeforeCss = jsNodes.Where(t => t.Line < cssNode.Line).OrderBy(t => t.Line);
                foreach (var jsNodeBefore in jsNodesBeforeCss)
                {
                    jsNodeBefore.Remove();
                    cssNode.ParentNode.InsertAfter(jsNodeBefore, cssNode);
                }
                counter++;
                document.Save(file);
            }

            return new List<AnalysisResult>
            {
                new AnalysisResult("", 0, Name, string.Format(Strings.Moved, counter), InfoType.Instance)
            };
        }

        #endregion
    }
}