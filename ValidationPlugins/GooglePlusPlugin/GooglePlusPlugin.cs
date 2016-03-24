using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using GooglePlusPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace GooglePlusPlugin
{
    /// <summary>
    ///     Manage the google plus link web data
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "GooglePlus")]
    [ExportMetadata("After", "Include")]
    public class GooglePlusPlugin : IValidation
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public GooglePlusPlugin()
        {
            View = new View(this);
        }

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;


        /// <summary>
        ///     Text to publisher textbloc in View
        /// </summary>
        public string PublisherText => Strings.Publisher;

        /// <summary>
        ///     Url to G+ page
        /// </summary>
        public string Publisher { get; set; }

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
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//link[@rel='publisher']");
                if (nodes == null)
                {
                    AnalysisResults.Add(NotFoundMessage(file));
                }
            }
            return AnalysisResults;
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
            if (!IsAutoFixeable || string.IsNullOrWhiteSpace(Publisher) ||!IsEnabled) return null;

            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            var counter = 0;
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);
                var headNode = document.DocumentNode.SelectSingleNode("//head");
                if (headNode != null)
                {
                    //Create meta description
                    var linkTag = document.CreateElement("link");

                    linkTag.Attributes.Add("rel", "publisher");
                    linkTag.Attributes.Add("href", Publisher);
                    //Add to head
                    headNode.AppendChild(linkTag);
                    counter++;
                    document.Save(file);
                }
            }

            return new List<AnalysisResult> {PublisherGenerate(counter)};
        }

        /// <summary>
        ///     Creates the generation message
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private AnalysisResult PublisherGenerate(int number)
        {
            return new AnalysisResult
            {
                File = "",
                Line = 0,
                PluginName = Name,
                Type = InfoType.Instance,
                Message = string.Format(Strings.Generated, number)
            };
        }

        #endregion
    }
}