namespace FtpClient.Protocols.ItemTypes
{
    /// <summary>
    ///     Interface to manage file system items
    /// </summary>
    public interface IProtocolItemType
    {
        /// <summary>
        ///     Name of the item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Icon to show from Segoe MDL2
        /// </summary>
        string Icon { get; set; }
    }
}