using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using HtmlAgilityPack;
using TextRatioPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace TextRatioPlugin
{
    /// <summary>
    ///     Plugin to calculate the text vs html ratio
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "TextRatio")]
    [ExportMetadata("After", "NormalizeCss")]
    public class TextRatioPlugin : IValidation
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
        public ICategoryType Type { get; } = SeoType.Instance;
        
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
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            long projectTotalSize = 0, projectTextSize = 0;
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                var fileInfo = new FileInfo(file);
                var totalSize = fileInfo.Length;
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//p");
                if (nodes == null) continue;
                var textSize = nodes.Sum(node => node.InnerText.Length);

                projectTotalSize += totalSize;
                projectTextSize += textSize;

                var ratio = textSize/(double) totalSize;
                if (ratio < 0.25 || ratio > 0.70)
                {
                    analysisResults.Add(new AnalysisResult(file, 0, Name, string.Format(Strings.BadRatio, ratio),
                        ErrorType.Instance));
                }
            }
            var projectRatio = projectTextSize/(double) projectTotalSize;
            analysisResults.Add(new AnalysisResult("", 0, Name, string.Format(Strings.ProjectRatio, projectRatio),
                InfoType.Instance));
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
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        #endregion
    }
}