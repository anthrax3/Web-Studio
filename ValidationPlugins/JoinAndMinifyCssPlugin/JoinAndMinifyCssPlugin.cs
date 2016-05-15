using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
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
    [Export(typeof(IValidation))]
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
            var analysisResults = new List<AnalysisResult>();

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
            if (!IsAutoFixeable || !IsEnabled) return null;
            if (string.IsNullOrWhiteSpace(Domain)) return new List<AnalysisResult>{new AnalysisResult("", 0, Name, Strings.DomainNotFound, ErrorType.Instance)};

            var results = new List<AnalysisResult>();
            FileModel.Domain = Domain;
            FileModel.ProjectPath = projectPath;
            var cssDictionary = new Dictionary<string, string>();
            var counter = 0;

            //it takes all html files
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories); 

            foreach (var file in filesToCheck) //For each html file
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);

                string resultCssFile = null;
                var cssFiles = document.DocumentNode.SelectNodes("//link[@rel='stylesheet']"); //Get styles
                if (cssFiles == null) continue;
                var key = GenerateKey(cssFiles);
                if (cssDictionary.ContainsKey(key.ToString())) // We already have the result file
                {
                    resultCssFile = cssDictionary[key.ToString()];
                    foreach (var cssFile in cssFiles) //Remove css files
                    {
                        cssFile.Remove();
                    }
                }
                else
                {
                    foreach (var cssFile in cssFiles) //Minify css files
                    {
                        var url = cssFile.GetAttributeValue("href", null);
                        if (url == null) continue;

                        var fileModel = new FileModel(url);
                        resultCssFile = counter + ".css";
                        fileModel.Minify(results, resultCssFile);
                        cssFile.Remove(); //Remove
                    }
                    cssDictionary[key.ToString()] = resultCssFile; //Add to dictionary
                    counter++;
                }
                AddNewCssFile(document, resultCssFile, file);
            }

            RemoveUnusedCssFiles(projectPath, cssDictionary);

            results.Add(new AnalysisResult("", 0, Name,
                string.Format(Strings.Compression, Stadistics.Ratio(projectPath, cssDictionary)), InfoType.Instance));
            return results;
        }

        /// <summary>
        /// Remove all unused CSS files
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="cssDictionary"></param>
        private void RemoveUnusedCssFiles(string projectPath, Dictionary<string, string> cssDictionary)
        {
            var cssFilesToRemove = Directory.GetFiles(projectPath, "*.css", SearchOption.AllDirectories);
            foreach (var cssFile in cssFilesToRemove)
            {
                var cssFileName = Path.GetFileName(cssFile);
                if (!cssDictionary.ContainsValue(cssFileName)) File.Delete(cssFile);
            }
        }

        /// <summary>
        /// Add the new css result file in the head tag
        /// </summary>
        /// <param name="document"></param>
        /// <param name="resultCssFile"></param>
        /// <param name="file"></param>
        private void AddNewCssFile(HtmlDocument document, string resultCssFile, string file)
        {
            var headNode = document.DocumentNode.SelectSingleNode("//head"); //Put the css after join and minify
            if (headNode != null)
            {
                //Create meta description
                var linkTag = document.CreateElement("link");

                linkTag.Attributes.Add("rel", "stylesheet");
                linkTag.Attributes.Add("href", Domain + "/css/" + resultCssFile);
                linkTag.Attributes.Add("type", "text/css");
                //Add to head
                headNode.AppendChild(linkTag);
                document.Save(file);
            }
        }

        /// <summary>
        /// This method generates a key from all css files. Example if we have 3 files: style.css, colors.css and main.css
        /// It returns style.css-colors.css-main.css
        /// </summary>
        /// <param name="cssFiles"></param>
        /// <returns></returns>
        private StringBuilder GenerateKey(HtmlNodeCollection cssFiles)
        {
            var key = new StringBuilder();
            foreach (var cssFile in cssFiles) //Create the key
            {
                var url = cssFile.GetAttributeValue("href", null);
                if (url == null) continue;
                key.Append("-" + url);
            }
            return key;
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