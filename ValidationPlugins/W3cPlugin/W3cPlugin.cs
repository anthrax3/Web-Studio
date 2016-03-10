using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using ValidationInterface;
using ValidationInterface.MessageTypes;
using W3cPlugin.Properties;

namespace W3cPlugin
{
    /// <summary>
    ///     Plugin that check the html files with the W3C specification
    /// </summary>
    [Export(typeof (IValidation))]
    public class W3cPlugin : IValidation
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
        public bool IsAutoFixeable { get; } = false;

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
            //Get the html files in the folder and subfolder
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            Console.WriteLine(filesToCheck.ToString());

            foreach (var file in filesToCheck)
            {
                var output = string.Empty;
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (directory != null)
                {
                    var processStartInfo = new ProcessStartInfo("cmd", "/c java -jar vnu.jar --format json " + file)
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
                        using (var streamReader = process.StandardError)
                        {
                            output = streamReader.ReadToEnd();
                        }
                        var account = JsonConvert.DeserializeObject<W3cResult>(output);
                        foreach (var message in account.Messages)
                        {
                            AnalysisResults.Add(GenerateResult(file, message));
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
        public void Fix(string projectPath)
        {
            //Do nothing
        }

        private AnalysisResult GenerateResult(string file, MessageClass message)
        {
            var analysisResult = new AnalysisResult
            {
                File = file,
                Line = message.LastLine,
                Message = message.Message,
                PluginName = Name
            };

            //Type
            if (message.Type.Equals("error"))
            {
                analysisResult.Type = ErrorType.Instance;
            }
            else
            {
                if (message.Type.Equals("info"))
                {
                    if (message.SubType.Equals("warning"))
                    {
                        analysisResult.Type = WarningType.Instance;
                    }
                    else
                    {
                        analysisResult.Type = InfoType.Instance;
                    }
                }
            }
            return analysisResult;
        }
    }
}