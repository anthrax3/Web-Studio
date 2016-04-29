using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FtpClient.Protocols.ItemTypes;
using Renci.SshNet;

namespace FtpClient.Protocols
{
    /// <summary>
    ///     Class to manage the SFTP protocol
    /// </summary>
    public class Sftp : IProtocol
    {
        private SftpClient _client;

        /// <summary>
        ///     Connect to remote host
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Connect(string server, string port, string user, string password)
        {
            _client = new SftpClient(server, int.Parse(port), user, password);
            _client.Connect();
            return _client.IsConnected;
        }

        /// <summary>
        ///     Get all items in this folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<ProtocolItem> ListDirectory(string path)
        {
            var items = new List<ProtocolItem>();
            var directoryItems = _client.ListDirectory(path);
            foreach (var item in directoryItems)
            {
                if (item.IsDirectory)
                {
                    items.Add(new ProtocolItem(item.Name, item.FullName, item.Length, item.LastWriteTime,
                        FolderType.Instance));
                    continue;
                }
                if (item.IsRegularFile)
                {
                    items.Add(new ProtocolItem(item.Name, item.FullName, item.Length, item.LastWriteTime,
                        FileType.Instance));
                }
            }
            items = new List<ProtocolItem>(items.Where(i => !(i.Name.Equals(".") || i.Name.Equals(".."))));
                //Remove . and .. element
            return items;
        }

        /// <summary>
        ///     Download a file (from remote to local)
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public bool DownloadFile(string sourcePath, string destinationPath)
        {
            try
            {
                Stream dataReceived = File.OpenWrite(destinationPath);
                _client.DownloadFile(sourcePath, dataReceived);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Method for upload a file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public bool UploadFile(string sourcePath, string destinationPath)
        {
            try
            {
                Stream dataToUpload = File.OpenRead(sourcePath);
                _client.UploadFile(dataToUpload, destinationPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Return the working directory
        /// </summary>
        /// <returns></returns>
        public string WorkingDirectory()
        {
            return _client.WorkingDirectory;
        }

        /// <summary>
        ///     Creates the directory in the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CreateDirectory(string path)
        {
            try
            {
                _client.CreateDirectory(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Disconnect
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            try
            {
                _client.Disconnect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}