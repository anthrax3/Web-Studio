using ValidationInterface.MessageTypes;

namespace ValidationInterface
{
    /// <summary>
    ///     Class to manage the output of each validation plugin.
    /// </summary>
    public class AnalysisResult
    {
        /// <summary>
        /// Constructor with values
        /// </summary>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <param name="plugin"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public AnalysisResult(string file, int line, string plugin, string message, IMessageType type)
        {
            File = file;
            Line = line;
            PluginName = plugin;
            Message = message;
            Type = type;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AnalysisResult()
        {
        }

        /// <summary>
        ///     Line where the error (or warning ...) was found
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        ///     Type of message
        /// </summary>
        public IMessageType Type { get; set; }

        /// <summary>
        ///     Message description
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     File where the validator produced a message
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Name of the plugin that it generated this result
        /// </summary>
        public string PluginName { get; set; }
    }
}