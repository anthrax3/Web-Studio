using System.Collections.Generic;

namespace W3cPlugin
{
    /// <summary>
    ///     Class to parse the output of w3c checker. Reference https://github.com/validator/validator/wiki/Output:-JSON
    /// </summary>
    public class W3cResult
    {
        /// <summary>
        ///     Messages list
        /// </summary>
        public List<MessageClass> Messages { get; set; }
    }

    /// <summary>
    ///     Class with the data of a message
    /// </summary>
    public class MessageClass
    {
        /// <summary>
        ///     Type of message
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Path to file where it was generated the message
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Line where the message was found
        /// </summary>
        public int LastLine { get; set; }

        /// <summary>
        ///     Last column where the message was found
        /// </summary>
        public int LastColumn { get; set; }

        /// <summary>
        ///     First column where the message was found
        /// </summary>
        public int FirstColumn { get; set; }

        /// <summary>
        ///     Message generated
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Where the message was found
        /// </summary>
        public string Extract { get; set; }

        /// <summary>
        ///     Highlighting start
        /// </summary>
        public int HiliteStart { get; set; }

        /// <summary>
        ///     Highlighting lenght
        /// </summary>
        public int HiliteLength { get; set; }

        /// <summary>
        ///     Message subtype
        /// </summary>
        public string SubType { get; set; }
    }
}