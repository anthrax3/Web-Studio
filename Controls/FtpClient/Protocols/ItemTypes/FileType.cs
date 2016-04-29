namespace FtpClient.Protocols.ItemTypes
{
    /// <summary>
    ///     Class to manage files
    /// </summary>
    public class FileType : IProtocolItemType
    {
        //Singleton
        private FileType()
        {
            //Ignore
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static FileType Instance { get; } = new FileType();

        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; set; } = "File";

        /// <summary>
        ///     Icon to show from Segoe MDL2
        /// </summary>
        public string Icon { get; set; } = "";
    }
}