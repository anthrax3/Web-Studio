using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace W3cPlugin
{
    [Export(typeof (IValidation))]
    public class W3cPlugin : IValidation
    {
        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public string Name { get; } = "W3C Validator";

        /// <summary>
        ///     Description
        /// </summary>
        public string Description { get; } = "This validator checks the markup validity of Web documents";

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        public Category Type { get; } = Category.Development;

        /// <summary>
        ///     Results of the check method.
        /// </summary>
        public List<AnalysisResult> AnalysisResults { get; private set; } = new List<AnalysisResult>();

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
            string [] filesToCheck =Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            Console.WriteLine(filesToCheck.ToString());

            foreach (string file in filesToCheck)
            {
                string output = string.Empty;
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                
                if (directory != null)
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c java -jar vnu.jar --format json " + file)
                    {
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        StandardErrorEncoding = Encoding.UTF8,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = directory
                    };

                    Process process = Process.Start(processStartInfo);

                    if (process != null)
                    {
                        using (StreamReader streamReader = process.StandardError)
                        {
                            output = streamReader.ReadToEnd();
                        }
                        W3cResult account = JsonConvert.DeserializeObject<W3cResult>(output);
                        foreach (MessageClass message in account.Messages)
                        {
                            AnalysisResults.Add(GenerateResult(file, message));
                        }
                    }
                }
            }
            return AnalysisResults;
        }

        private AnalysisResult GenerateResult(string file, MessageClass message)
        {
            AnalysisResult analysisResult = new AnalysisResult
            {
                File = file,
                Line = message.LastLine,
                Message = message.Message
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

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public void Fix(string projectPath)
        {
            //Do nothing
        }
    }
}
