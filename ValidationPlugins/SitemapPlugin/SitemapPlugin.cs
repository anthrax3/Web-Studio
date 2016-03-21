using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using SitemapPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace SitemapPlugin
{
    /// <summary>
    ///     Plugin to check and generate the Sitemap file
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Sitemap")]
    [ExportMetadata("After", "Robot")]
    public class SitemapPlugin : IValidation
    {
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SitemapPlugin()
        {
            View = new View(this);
        }
         
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
            AnalysisResults.Clear();
            if (!IsEnabled) return AnalysisResults;

            var filesToCheck = Directory.GetFiles(projectPath, "*sitemap*.xml", SearchOption.AllDirectories);
            if (filesToCheck.Length == 0)
            {
                AnalysisResults.Add(new AnalysisResult
                {
                    File = "",
                    Line = 0,
                    PluginName = Name,
                    Type = ErrorType.Instance,
                    Message = Strings.NotFound
                });
            }
            else
            {
               AnalysisResults.Add(new AnalysisResult
                {
                    File = "",
                    Line = 0,
                    PluginName = Name,
                    Type = InfoType.Instance,
                    Message = string.Format(Strings.HaveFound, filesToCheck.Length)
                });
            }
           
            return AnalysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || String.IsNullOrEmpty(Domain)) return null;

            var htmlFiles = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);

            StringBuilder sitemap = new StringBuilder();
            sitemap.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>"); //Header
            sitemap.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");
            foreach (var file in htmlFiles)
            {
                string relativeUrl = (file.Replace(projectPath,String.Empty)).Replace(@"\",@"/"); //change file separator
                sitemap.AppendLine(@"<url><loc>" + Domain + relativeUrl + @"</loc></url>");
            }
            sitemap.Append(@"</urlset>");
            File.WriteAllText(Path.Combine(projectPath,"sitemap.xml"),sitemap.ToString());

            File.AppendAllText(Path.Combine(projectPath, "robots.txt"),@"Sitemap: "+Domain+@"/sitemap.xml"); //Update robots.txt

            return null;



        }

        #region Custom Properties

        /// <summary>
        /// Display info about domain property
        /// </summary>
        public string DomainName => Strings.DomainName;

        /// <summary>
        /// Full path to root file
        /// </summary>
        public string Domain { get; set; }

        #endregion
    }
}