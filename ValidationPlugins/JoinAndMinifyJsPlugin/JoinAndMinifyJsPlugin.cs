using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows.Controls;
using HtmlAgilityPack;
using JoinAndMinifyJsPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace JoinAndMinifyJsPlugin
{
    /// <summary>
    ///     Join all JS files and minify it
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "JoinAndMinifyJs")]
    [ExportMetadata("After", "Include")]
    public class JoinAndMinifyJsPlugin : IValidation
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
            //it takes all js files
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);

            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);


                var cssFiles = document.DocumentNode.SelectNodes("//script[@src]");
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
            if (!IsAutoFixeable || !IsEnabled) return null;
            if (string.IsNullOrWhiteSpace(Domain))
                return new List<AnalysisResult>
                {
                    new AnalysisResult("", 0, Name, Strings.DomainNotFound, ErrorType.Instance)
                };


            var results = new List<AnalysisResult>();
            //it takes all js files
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            var jsDictionary = new Dictionary<string, string>();
            FileModel.Domain = Domain;
            var counter = 0;


            foreach (var file in filesToCheck) //For each html file
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);

                string resultJsFile = null;
                var jsFiles = document.DocumentNode.SelectNodes("//script[@src]"); //Get js
                if (jsFiles == null) continue; //No js files found
                var key = new StringBuilder();
                foreach (var jsFile in jsFiles) //Create the key
                {
                    var url = jsFile.GetAttributeValue("src", null);
                    if (url == null) continue;
                    key.Append("-" + url);
                }
                if (jsDictionary.ContainsKey(key.ToString())) // We already have the result file
                {
                    resultJsFile = jsDictionary[key.ToString()];
                    foreach (var jsFile in jsFiles) //Remove js files
                    {
                        jsFile.Remove();
                    }
                }
                else
                {
                    foreach (var jsFile in jsFiles) //Minify js files
                    {
                        var url = jsFile.GetAttributeValue("src", null);
                        if (url == null) continue;

                        var fileModel = new FileModel(url, projectPath);
                        resultJsFile = counter + ".js";
                        fileModel.Minify(results, resultJsFile);
                        jsFile.Remove(); //Remove
                    }
                    jsDictionary[key.ToString()] = resultJsFile; //Add to dictionary
                    counter++;
                }

                var headNode = document.DocumentNode.SelectSingleNode("//head"); //Put the js after join and minify
                if (headNode != null)
                {
                    //Create meta description
                    var linkTag = document.CreateElement("script");
                    linkTag.Attributes.Add("src", Domain + "/js/" + resultJsFile);
                    //Add to head
                    headNode.AppendChild(linkTag);
                    document.Save(file);
                }
            }

            //Remove unused JS files
            var jsFilesToRemove = Directory.GetFiles(projectPath, "*.js", SearchOption.AllDirectories);
            foreach (var jsFile in jsFilesToRemove)
            {
                var jsFileName = Path.GetFileName(jsFile);
                if (!jsDictionary.ContainsValue(jsFileName)) File.Delete(jsFile);
            }

            results.Add(new AnalysisResult("", 0, Name,
                string.Format(Strings.Compression, Stadistics.Ratio(projectPath)), InfoType.Instance));
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