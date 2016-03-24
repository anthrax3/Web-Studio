using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.IO;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using DescriptionPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface.MessageTypes;

namespace DescriptionPlugin
{
    /// <summary>
    /// Plugin that checks and generate a page description
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "Description")]        
    [ExportMetadata("After", "Include")]             
    public class DescriptionPlugin : IValidation 
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DescriptionPlugin()
        {
            View = new View(this);
        }

        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

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
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//meta[@name='description']");
                if (nodes == null)
                {
                    AnalysisResults.Add(NotFoundMessage(file));
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        var content = node.GetAttributeValue("content", null);
                        if (content != null)
                        {
                            if (content.Length < 130 | content.Length > 155)
                            {
                                AnalysisResults.Add(DescriptionLength(file, node.Line));
                            }
                        }
                    }
                }
            }
            return AnalysisResults;
        }

        /// <summary>
        /// Creates a not found message
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
        /// Creates a length warning
        /// </summary>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private AnalysisResult DescriptionLength(string file, int line)
        {
            return new AnalysisResult
            {
              File = file,
              Line = line,
              PluginName = Name,
              Type = WarningType.Instance,
              Message = Strings.DescriptionLength
            };
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            
            List<AnalysisResult> list = new List<AnalysisResult>();
            int descriptionsCreated = 0;
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//meta[@name='description']");
                if (nodes == null) //Add node
                {
                    var paragraph = document.DocumentNode.SelectSingleNode("//p");
                    if (paragraph == null)
                    {
                      list.Add(NoParagraphMessage(file));  
                    }
                    else
                    {
                        var headNode = document.DocumentNode.SelectSingleNode("//head");
                        if (headNode != null)
                        {
                            //Create meta description
                            HtmlNode metaDescription = document.CreateElement("meta");
                            
                            metaDescription.Attributes.Add("name","description");
                            metaDescription.Attributes.Add("content",paragraph.InnerText);
                            //Add to head
                            headNode.AppendChild(metaDescription);
                            descriptionsCreated++;
                            document.Save(file);
                        }
                    }
                }
            }

            list.Add(DescriptionsGenerated(descriptionsCreated));
            return list;
        }

        /// <summary>
        /// Creates the generation message
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private AnalysisResult DescriptionsGenerated(int number)
        {
            return new AnalysisResult
            {
                File = "",
                Line = 0,
                PluginName = Name,
                Type = InfoType.Instance,
                Message =  String.Format(Strings.Generated,number)
            };
        }

        /// <summary>
        /// Creates the error message when no paragraph is found
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalysisResult NoParagraphMessage(string file)
        {
            return new AnalysisResult
            {
               File = file,
               Line = 0,
               PluginName = Name,
               Type = WarningType.Instance,
               Message = Strings.ParagraphNotFound 
            };
        }
        #endregion
    }
}
