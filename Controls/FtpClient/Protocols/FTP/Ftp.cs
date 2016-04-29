using System;
using System.Collections.Generic;
using System.Net;
using AlexPilotti.FTPS.Client;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols.FTP
{
    /// <summary>
    /// Class to manage the FTP protocol
    /// </summary>
    public class Ftp  : IProtocol
    {
        /// <summary>
        /// Client to manage the protocol
        /// </summary>
        protected FTPSClient _client;

        /// <summary>
        ///     Connect to remote host
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool Connect(string server, string port, string user, string password)
        {  
            try
            {
                _client = new FTPSClient();
                _client.Connect(server, new NetworkCredential(user, password),ESSLSupportMode.ClearText); 
                return true;
            }
            catch (Exception)
            {
                return false;
            }
          
        }

        /// <summary>
        ///     Get all items in the directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<ProtocolItem> ListDirectory(string path)
        {
            return FtpParser.Parse(_client.GetDirectoryListUnparsed(path),path);  
        }

        /// <summary>
        ///     Method for download a file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public bool DownloadFile(string sourcePath, string destinationPath)
        {
            try
            {
                return _client.GetFile(sourcePath, destinationPath) > 0;
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
                return _client.PutFile(sourcePath, destinationPath) > 0; 
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
            return _client.GetCurrentDirectory();
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
                _client.MakeDir(path);
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
                _client.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}