using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using HtmlAgilityPack;
using JoinAndMinifyCssPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace JoinAndMinifyCssPlugin
{
    /// <summary>
    ///     Join all CSS files and minify it
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "JoinAndMinifyCss")]
    [ExportMetadata("After", "PrintCss")]
    public class JoinAndMinifyCssPlugin : IValidation
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
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();
       
            if (!IsEnabled) return analysisResults;
            //it takes all css files
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);

            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);


                var cssFiles = document.DocumentNode.SelectNodes("//link[@rel='stylesheet']");
                if (cssFiles == null) continue;
                if (cssFiles.Count > 1)
                {
                    analysisResults.Add(new AnalysisResult(file, 0, Name, Strings.TooFiles, WarningType.Instance));
                }
            }


            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled || string.IsNullOrWhiteSpace(Domain)) return null;
            var results = new List<AnalysisResult>();
            //it takes all css files
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            var cssUrls = new HashSet<string>();
            FileModel.Domain = Domain;

            foreach (var file in filesToCheck) //For each html file
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);


                var cssFiles = document.DocumentNode.SelectNodes("//link[@rel='stylesheet']"); //Get styles
                if (cssFiles == null) continue;
                foreach (var cssFile in cssFiles) //Minify css files
                {
                    var url = cssFile.GetAttributeValue("href", null);
                    if (url == null) continue;
                    if (!cssUrls.Add(url)) continue; //that css was minified before

                    var fileModel = new FileModel(url, projectPath);
                    fileModel.Minify(results);
                    cssFile.Remove(); //Remove
                }

                var headNode = document.DocumentNode.SelectSingleNode("//head"); //Put the css after join and minify
                if (headNode != null)
                {
                    //Create meta description
                    var linkTag = document.CreateElement("link");

                    linkTag.Attributes.Add("rel", "stylesheet");
                    linkTag.Attributes.Add("href", Domain + "/css/style.css");
                    linkTag.Attributes.Add("type", "text/css");
                    //Add to head
                    headNode.AppendChild(linkTag);
                    document.Save(file);
                }
            }

            results.Add(new AnalysisResult("", 0, Name,
                string.Format(Strings.Compression, Stadistics.Ratio(projectPath)), InfoType.Instance));
            return results;
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