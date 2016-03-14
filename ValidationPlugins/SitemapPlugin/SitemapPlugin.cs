using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using SitemapPlugin.Properties;
using ValidationInterface;
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
        public Category Type { get; } = Category.Development;

        /// <summary>
        ///     Results of the check method.
        /// </summary>
        public List<AnalysisResult> AnalysisResults { get; } = new List<AnalysisResult>();

        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        public bool IsAutoFixeable { get; } = true;

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
        public void Fix(string projectPath)
        {
            //Requiere mas datos, se necesita ruta completa de dominio
        }
    }
}