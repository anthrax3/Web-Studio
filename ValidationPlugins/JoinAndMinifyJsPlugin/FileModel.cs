using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using JoinAndMinifyJsPlugin.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;
using WebMarkupMin.MsAjax.Minifiers;

namespace JoinAndMinifyJsPlugin
{
    /// <summary>
    ///     Class to manage and minify a file
    /// </summary>
    public class FileModel
    {
        /// <summary>
        ///     The domain of the project
        /// </summary>
        public static string Domain;

        /// <summary>
        /// Full path to project
        /// </summary>
        public static string ProjectPath;

        private string _file;

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="file"></param>
        public FileModel(string file)
        {
            _file = NormalizeUrl(file);
        }

        /// <summary>
        ///     method to minify the file
        /// </summary>
        /// <param name="results"></param>
        /// <param name="resultJs"></param>
        public void Minify(List<AnalysisResult> results, string resultJs)
        {
            try
            {
                string fileToProcess;
                if (InternalLinkCheck()) //Local file
                {
                    if (_file[0].Equals('/'))
                    {
                        _file = _file.Replace('/', Path.DirectorySeparatorChar);
                        _file = _file.Substring(1); //Remove first char
                    }
                    fileToProcess = Path.Combine(ProjectPath, _file);
                }
                else //Download
                {
                    var webClient = new WebClient();
                    webClient.DownloadFile(_file, "temp");
                    fileToProcess = "temp";
                }
                var fileInfo = new FileInfo(fileToProcess);
                Stadistics.TotalJsSize += fileInfo.Length; //Get size without minify   
                var content = File.ReadAllText(fileToProcess);
                var minifier = new MsAjaxJsMinifier();
                var minifyResult = minifier.Minify(content, false);
                //Save file
                File.AppendAllText(Path.Combine(ProjectPath, "js", resultJs), minifyResult.MinifiedContent);

                //Errors
                results.AddRange(
                    minifyResult.Errors.Select(
                        error =>
                            new AnalysisResult(_file.Replace("release", "src"), error.LineNumber, Strings.Name,
                                error.Message, ErrorType.Instance)));
                results.AddRange(
                    minifyResult.Warnings.Select(
                        warning =>
                            new AnalysisResult(_file.Replace("release", "src"), warning.LineNumber, Strings.Name,
                                warning.Message, WarningType.Instance)));
            }
            catch (Exception)
            {
                results.Add(new AnalysisResult(_file, 0, Strings.Name, Strings.FileProblem, ErrorType.Instance));
            }
        }

        /// <summary>
        ///     This method makes all internal urls to relative urls
        /// </summary>
        /// <returns></returns>
        private string NormalizeUrl(string link)
        {
            return link.Replace(Domain + "/", string.Empty);
        }

        /// <summary>
        ///     Marks link as internal or external
        /// </summary>
        /// <returns></returns>
        private bool InternalLinkCheck()
        {
            if (string.IsNullOrWhiteSpace(_file)) return false; //Domain url
            var httpRegex = new Regex("http.*");
            if (httpRegex.IsMatch(_file)) //External url
            {
                return false;
            }
            return true;
        }
    }
}