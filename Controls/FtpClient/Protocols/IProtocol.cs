using System.Collections.Generic;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols
{
    /// <summary>
    ///     Protocol for item transmission
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        ///     Connect to remote host
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Connect(string server, string port, string user, string password);

        /// <summary>
        ///     Get all items in the directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        List<ProtocolItem> ListDirectory(string path);

        /// <summary>
        ///     Method for download a file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        bool DownloadFile(string sourcePath, string destinationPath);

        /// <summary>
        ///     Method for upload a file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        bool UploadFile(string sourcePath, string destinationPath);

        /// <summary>
        ///     Return the working directory
        /// </summary>
        /// <returns></returns>
        string WorkingDirectory();

        /// <summary>
        ///     Creates the directory in the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool CreateDirectory(string path);

        /// <summary>
        ///     Disconnect
        /// </summary>
        /// <returns></returns>
        bool Disconnect();
    }
}