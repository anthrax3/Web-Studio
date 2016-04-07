using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using FacebookPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace FacebookPlugin
{
    /// <summary>
    ///     Manages fb metadata for improve fb sharing
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Facebook")]
    [ExportMetadata("After", "Description")]
    public class FacebookPlugin : IValidation
    {  
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

        /// <summary>
        ///     Display info about domain property
        /// </summary>
        public string DomainName => Strings.DomainName;

        /// <summary>
        ///     Full path to root file
        /// </summary>
        public string Domain { get; set; }

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
            List<AnalysisResult> analysisResults = new List<AnalysisResult>();
            analysisResults.Clear();
            if (!IsEnabled) return analysisResults;

            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//meta[@property='og:title']");
                if (nodes == null)
                {
                    analysisResults.Add(NotFoundMessage(file));
                }
            }

            return analysisResults;
        }

        /// <summary>
        ///     Creates a not found message
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalysisResult NotFoundMessage(string file)
        {
            return new AnalysisResult
            {
                File = file,
                Line = 0,
                PluginName = Name,
                Type = ErrorType.Instance,
                Message = Strings.NotFound
            };
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;

            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            var list = new List<AnalysisResult>();
            var counter = 0;
            foreach (var file in filesToCheck)
            {
                var utils = new OpenGraph(file, Domain, projectPath);
                list.AddRange(utils.AddTags());
                counter++;
            }
            list.Add(new AnalysisResult("", 0, Name, string.Format(Strings.Generated, counter), InfoType.Instance));
            return list;
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