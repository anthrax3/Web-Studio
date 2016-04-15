using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using LinksPlugin.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace LinksPlugin
{
    /// <summary>
    ///     Class to manage the links
    /// </summary>
    public class LinkModel
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <param name="link"></param>
        public LinkModel(string file, int line, string link)
        {
            File = file;
            Line = line;
            Link = NormalizeUrl(link);
            IsInternal = InternalLinkCheck();
        }

        /// <summary>
        ///     Domain value
        /// </summary>
        public static string Domain { get; set; }

        /// <summary>
        ///     File where the link is located
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Like where the link is located
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        ///     The link itself
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     This is an internal link
        /// </summary>
        public bool IsInternal { get; set; }

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
            if (string.IsNullOrWhiteSpace(Link)) return false; //Domain url
            var httpRegex = new Regex("http.*");
            if (httpRegex.IsMatch(Link)) //External url
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Check if the url has a question mark
        /// </summary>
        /// <returns></returns>
        public AnalysisResult HasQuestionMark()
        {
            if (Link.Contains("?"))
            {
                return new AnalysisResult(File, Line, Strings.Name, Strings.QuestionMark, WarningType.Instance);
            }
            return null;
        }

        /// <summary>
        ///     Checks the url lenght
        /// </summary>
        /// <returns></returns>
        public AnalysisResult UrlLength()
        {
            if (IsInternal)
            {
                if ((Domain + Link).Length > 60)
                {
                    return new AnalysisResult(File, Line, Strings.Name, Strings.Length, WarningType.Instance);
                }
            }
            return null;
        }

        /// <summary>
        ///     Checks if the url is seo friendly
        /// </summary>
        /// <returns></returns>
        public AnalysisResult SeoUrlCheck()
        {
            if (IsInternal)
            {
                var rule = new Regex("[a-zA-Z0-9-]+");
                if (!rule.IsMatch(Link))
                {
                    return new AnalysisResult(File, Line, Strings.Name, Strings.SeoUrl, WarningType.Instance);
                }
            }
            return null;
        }

        /// <summary>
        ///     Checks broken links
        /// </summary>
        /// <returns></returns>
        public AnalysisResult BrokenUrlCheck()
        {
            if (!IsInternal)
            {
                try
                {
                    var request = WebRequest.Create(Link) as HttpWebRequest;
                    request.Timeout = 5000; //set the timeout to 5 
                    request.Method = "HEAD"; //Get only the header information -- no need to download any content

                    var response = request.GetResponse() as HttpWebResponse;

                    var statusCode = (int) response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400) //Good requests
                    {
                        return null;
                    }
                    if (statusCode >= 500 && statusCode <= 510) //Errors
                    {
                        return new AnalysisResult(File, Line, Strings.Name, Strings.BrokenUrl, ErrorType.Instance);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                    {
                        return new AnalysisResult(File, Line, Strings.Name, Strings.BrokenUrl, ErrorType.Instance);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                return null;
            }
            return null;
        }

        /// <summary>
        ///     Check if the local file exists
        /// </summary>
        /// <param name="filesToCheck"></param>
        /// <returns></returns>
        public AnalysisResult CheckLocalFiles(List<LocalFile> filesToCheck)
        {
            if (IsInternal)
            {
                var internalUrls = filesToCheck.Count(file => file.File == Link); //Check if the internal url exists
                if (internalUrls == 0)
                {
                    return new AnalysisResult(File, Line, Strings.Name, Strings.LocalFileNotFound, ErrorType.Instance);
                }
                foreach (var file in filesToCheck.Where(file => file.File == Link)) //Mark as referenced
                {
                    file.IsReferenced = true;
                }
            }
            return null;
        }
    }
}