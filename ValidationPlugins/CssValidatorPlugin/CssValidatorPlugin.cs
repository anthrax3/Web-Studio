using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using CssValidatorPlugin.Properties;
using Newtonsoft.Json;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace CssValidatorPlugin
{
    /// <summary>
    ///     Class to validate local css files
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "CssValidator")]
    [ExportMetadata("After", "Include")]
    public class CssValidatorPlugin : IValidation
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public CssValidatorPlugin()
        {
            View = new View(this);
        }

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
            var filesToCheck = Directory.GetFiles(projectPath, "*.css", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (directory != null)
                {
                    var processStartInfo = new ProcessStartInfo("cmd", "/c java -jar css-validator.jar --output=json --lang=es file:" + file)  //Run cmd in background
                    {
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        StandardErrorEncoding = Encoding.UTF8,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = directory
                    };

                    var process = Process.Start(processStartInfo);

                    if (process != null)
                    {
                        string output;
                        using (var streamReader = process.StandardOutput)
                        {
                            output = streamReader.ReadToEnd();
                        }
                        output = output.Remove(0, 80);
                        try
                        {
                            var result = JsonConvert.DeserializeObject<CssValidatorResult>(output);
                            if (result.cssvalidation.errors != null)
                            {
                                foreach (Error error in result.cssvalidation.errors)
                                {
                                    AnalysisResults.Add(new AnalysisResult(file.Replace("release", "src"), error.line, Name, error.message, ErrorType.Instance));
                                }
                            }
                            if (result.cssvalidation.warnings != null)
                            {
                                foreach (Warning warning in result.cssvalidation.warnings)
                                {
                                    AnalysisResults.Add(new AnalysisResult(file.Replace("release", "src"), warning.line, Name, warning.message, WarningType.Instance));
                                }
                            }
                        }
                        catch (Exception)
                        {
                           //TODO
                           
                        }
                       
                      
                    }
                }
            }

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