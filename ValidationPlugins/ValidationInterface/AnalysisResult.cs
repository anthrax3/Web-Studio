using ValidationInterface.MessageTypes;

namespace ValidationInterface
{
    /// <summary>
    ///     Class to manage the output of each validation plugin.
    /// </summary>
    public class AnalysisResult
    {
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