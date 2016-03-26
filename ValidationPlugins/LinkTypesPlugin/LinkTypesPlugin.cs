using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.IO;
using HtmlAgilityPack;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using LinkTypesPlugin.Properties;
using ValidationInterface.MessageTypes;

namespace LinkTypesPlugin
{
    /// <summary>
    ///  Counts follow and no follow links
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "LinkTypes")]          
    [ExportMetadata("After", "Links")]           
    public class LinkTypesPlugin : IValidation  
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LinkTypesPlugin()
        {
            View = new View(this);
        }



        #region IValidation
        /// <summary>
        /// View showed when you select the plugin
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
        public ICategoryType Type { get; } = SeoType.Instance;

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
            int follow = 0, noFollow = 0;
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//a"); //Get all a tags
                if (nodes == null) continue;
                foreach (HtmlNode node in nodes)
                {
                    var relAtribute = node.GetAttributeValue("rel", null);
                    if (relAtribute != null && relAtribute == "nofollow") noFollow++;
                    else
                    {
                        follow++;
                    }
                }
            }
            AnalysisResults.Add(new AnalysisResult("",0,Name,String.Format(Strings.Found,follow,noFollow),InfoType.Instance));
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
