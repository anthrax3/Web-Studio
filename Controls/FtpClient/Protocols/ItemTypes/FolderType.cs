namespace FtpClient.Protocols.ItemTypes
{
    /// <summary>
    ///     Class to manage folders
    /// </summary>
    public class FolderType : IProtocolItemType
    {
        //Singleton
        private FolderType()
        {
            //Ignore
        }

        /// <summary>
        ///     Singleton
        /// </summary>
        public static FolderType Instance { get; } = new FolderType();

        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; set; } = "Folder";

        /// <summary>
        ///     Icon to show from Segoe MDL2
        /// </summary>
        public string Icon { get; set; } = "";
    }
}