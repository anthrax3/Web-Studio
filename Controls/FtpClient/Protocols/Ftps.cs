using System;
using System.Net;
using AlexPilotti.FTPS.Client;
using FtpClient.Protocols.FTP;

namespace FtpClient.Protocols
{
    /// <summary>
    ///     Class to manage FTPS protocol
    /// </summary>
    public class Ftps : Ftp
    {
        /// <summary>
        ///     Connect to remote host
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool Connect(string server, string port, string user, string password)
        {
            try
            {
                _client = new FTPSClient();
                //it doesn't validate the certificate due to some errors with selfsigned certificates
                _client.Connect(server, new NetworkCredential(user, password), ESSLSupportMode.All,
                    (sender, certificate, chain, errors) => true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}