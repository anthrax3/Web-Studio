namespace FtpClient
{
    /// <summary>
    /// Class to save data about the site connection
    /// </summary>
    public class Site
    {
        /// <summary>
        /// Url to server
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// User for loggin
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Port to connect
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Mode of transmission
        /// </summary>
        public string ProtocolMode { get; set; }
    }
}
