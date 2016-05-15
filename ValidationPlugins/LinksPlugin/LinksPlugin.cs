using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using HtmlAgilityPack;
using LinksPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace LinksPlugin
{
    /// <summary>
    ///     Class to check the links of the web page
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Links")]
    [ExportMetadata("After", "Include")]
    public class LinksPlugin : IValidation
    {
       

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
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;
            if (string.IsNullOrWhiteSpace(Domain))
                return new List<AnalysisResult>
                {
                    new AnalysisResult("", 0, Name, Strings.DomainNotFound, ErrorType.Instance)
                };

            var links = new HashSet<LinkModel>();
            LinkModel.Domain = Domain;
            SearchAndAddLinks(projectPath, links);
            var filesToCheck = Directory.GetFiles(projectPath, "*.*", SearchOption.AllDirectories); //Get all files

            var localFiles = filesToCheck.Select(file => new LocalFile(file, projectPath)).ToList();


            foreach (var link in links)
            {
                var result = link.HasQuestionMark();
                if (result != null) analysisResults.Add(result);
                result = link.UrlLength();
                if (result != null) analysisResults.Add(result);
                result = link.SeoUrlCheck();
                if (result != null) analysisResults.Add(result);
                result = link.BrokenUrlCheck();
                if (result != null) analysisResults.Add(result);
                result = link.CheckLocalFiles(localFiles);
                if (result != null) analysisResults.Add(result);
            }
            foreach (var file in localFiles) // Create a warning message for each unreferenced file
            {
                if (!file.IsReferenced)
                {
                    analysisResults.Add(new AnalysisResult(file.FullPath, 0, Name, Strings.CanRemoveFile,
                        WarningType.Instance));
                }
            }

            return analysisResults;
        }


        private void SearchAndAddLinks(string projectPath, HashSet<LinkModel> links)
        {
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck) //Search for links and add all
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//@src | //@href");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var url = node.GetAttributeValue("src", null);
                        if (url != null) links.Add(new LinkModel(file, node.Line, url));
                        url = node.GetAttributeValue("href", null);
                        if (url != null) links.Add(new LinkModel(file, node.Line, url));
                    }
                }
            }
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