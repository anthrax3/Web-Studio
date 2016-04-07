using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Error404PagePlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace Error404PagePlugin
{
    /// <summary>
    ///     Plugin to check and generate error 404 page
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Error404Page")]
    [ExportMetadata("After", "Htaccess")]
    public class Error404Page : IValidation
    {
       
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix; 

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
            List<AnalysisResult> analysisResults = new List<AnalysisResult>();
            analysisResults.Clear();
            if (!IsEnabled) return analysisResults;

            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            if (!File.Exists(htaccessPath))
            {
                analysisResults.Add(new AnalysisResult
                {
                    PluginName = Name,
                    File = "",
                    Line = 0,
                    Type = ErrorType.Instance,
                    Message = Strings.NotFound
                });
            }
            else
            {
                var content = File.ReadAllText(htaccessPath);
                var match = Regex.Match(content, @"ErrorDocument 404 .*");
                if (!match.Success)
                {
                    analysisResults.Add(new AnalysisResult
                    {
                        PluginName = Name,
                        File = htaccessPath,
                        Line = 0,
                        Type = ErrorType.Instance,
                        Message = Strings.TagNotFound
                    });
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
            var configurationContent =
                @"# ----------------------------------------------------------------------
# | Custom error messages/pages                                        |
# ----------------------------------------------------------------------

# Customize what Apache returns to the client in case of an error.
# https://httpd.apache.org/docs/current/mod/core.html#errordocument

# ErrorDocument 404 /404.html";

            if (!IsAutoFixeable || !IsEnabled) return null;
            var htaccessPath = Path.Combine(projectPath, ".htaccess");
            File.AppendAllText(htaccessPath, configurationContent);

            //Copy the 404 error page
            var source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "404.html");
            var destination = Path.Combine(projectPath, "404.html");
            File.Copy(source, destination);
            List<AnalysisResult> list = new List<AnalysisResult> {ErrorPageGenerated(destination)};
            return list;
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        /// <summary>
        /// Creates the Error 404 page message
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private AnalysisResult ErrorPageGenerated(string file)
        {
            return new AnalysisResult
            {
                File = file,
                Line = 0,
                PluginName = Name,
                Type = InfoType.Instance,
                Message = Strings.Generated
            };
        }

        #endregion
    }
}