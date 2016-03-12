using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using IncludePlugin.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace IncludePlugin
{
    /// <summary>
    /// Class to manage each file to check
    /// </summary>
    public class FileToCheck
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Document to parse
        /// </summary>
        private HtmlDocument Document { get; set; }
        /// <summary>
        /// This file has been included in other files
        /// </summary>
        public bool IsIncludedFile { get; set; }

        /// <summary>
        /// Default constructor, load the file
        /// </summary>
        /// <param name="file"></param>
        public FileToCheck(string file)
        {
            FilePath = file;
           Document = new HtmlDocument();
           Document.Load(file);
        }

        /// <summary>
        /// Algorithm to include the content of a file inside other file
        /// </summary>
        /// <param name="analysisResults"></param>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public int MakeInclusion(List<AnalysisResult> analysisResults, string pluginName)
        {
            int numIncludes = 0;
            var includeNodes = Document.DocumentNode.SelectNodes("//include");  //Search for include tag
            if (includeNodes == null) return 0;

            foreach (HtmlNode node in includeNodes) //Process the include tags
            {
                var htmlSource = node.GetAttributeValue("src", null);
                if (htmlSource == null) //Include without src --> Error msg
                {
                    analysisResults.Add(new AnalysisResult
                    {
                        File = FilePath,
                        Line = node.Line,
                        PluginName = pluginName,
                        Type = ErrorType.Instance,
                        Message = Strings.SrcNotFound
                    });
                }
                else    //Do the inclusion
                {
                    var temp = Document.CreateElement("temp");  //Temporal tag, we use it to load other tags inside it
                    var documentToInclude = File.ReadAllText(Path.Combine(Path.GetDirectoryName(FilePath),htmlSource)); //Relative url
                    temp.InnerHtml = documentToInclude;
                    var current = node;
                    foreach (var childNode in temp.ChildNodes) //Put each tag after the last tag
                    {
                        node.ParentNode.InsertAfter(childNode, current);
                        current = childNode;
                    }
                    node.ParentNode.RemoveChild(node); //Remove include tag
                    numIncludes++;
                }
            }

            //Save the File
            Document.Save(FilePath);
            return numIncludes;
        }

        /// <summary>
        /// Mark this file as IncludedFile if there is not html tag
        /// </summary>
        public void IncludedFile()
        {
            var headTag = Document.DocumentNode.SelectNodes("//html");
            IsIncludedFile = headTag == null;    //A html file without head tag is a included file
        }
    }
}