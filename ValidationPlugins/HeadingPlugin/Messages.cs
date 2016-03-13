using System;
using HeadingPlugin.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace HeadingPlugin
{
    /// <summary>
    /// Class to generate the messages
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// tag h1 not found in the file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AnalysisResult H1NotFound(string file)
        {
            return new AnalysisResult
            {
                PluginName = Strings.Name,
                File = file,
                Type = ErrorType.Instance,
                Message = Strings.H1NotFound
            };
        }

        /// <summary>
        /// More than one h1 tags found in the file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AnalysisResult ManyH1Found(string file)
        {
            return new AnalysisResult
            {
                PluginName = Strings.Name,
                File = file,
                Type = ErrorType.Instance,
                Message = Strings.ManyH1Found
            };
        }

        /// <summary>
        /// The h2 tag was not found
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AnalysisResult H2NotFound(string file)
        {
            return new AnalysisResult
            {
                PluginName = Strings.Name,
                File = file,
                Type = WarningType.Instance,
                Message = Strings.H2NotFound
            };
        }

        /// <summary>
        /// Recount message
        /// </summary>
        /// <param name="h1"></param>
        /// <param name="h2"></param>
        /// <param name="h3"></param>
        /// <returns></returns>
        public static AnalysisResult TagsCount(int h1, int h2, int h3)
        {
            return new AnalysisResult
            {
                PluginName = Strings.Name,
                File = "",
                Type = InfoType.Instance,
                Message = Strings.Found + " H1="+h1 + " H2="+h2+" H3="+h3
            };
        }
    }
}