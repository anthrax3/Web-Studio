using System.ComponentModel.Composition;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SocialCheckInterface;

namespace DomainCheck
{
    [Export(typeof (ISocialCheck))]
    public class DomainCheck : ISocialCheck
    {
        /// <summary>
        ///     whois server url
        /// </summary>
        private readonly string _whoisServer = "whois.verisign-grs.com";

        /// <summary>
        ///     type of service
        /// </summary>
        public string ServiceName { get; set; } = "URL";

        /// <summary>
        ///     Username in service
        /// </summary>
        public string NameInService { get; set; }

        /// <summary>
        ///     Is available
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        ///     Check .com domain availability
        /// </summary>
        /// <param name="name"></param>
        public void CheckAvailability(string name)
        {
            NameInService = "http://www." + name + ".com";

            using (var whoisClient = new TcpClient())
            {
                whoisClient.Connect(_whoisServer, 43);
                var query = "domain " + name + ".com\r\n";
                var queryBytes = Encoding.ASCII.GetBytes(query.ToCharArray());

                Stream whoisStream = whoisClient.GetStream();
                whoisStream.Write(queryBytes, 0, queryBytes.Length); //Send query to whois server

                var whoiStreamReader = new StreamReader(whoisClient.GetStream(), Encoding.ASCII);

                var response = whoiStreamReader.ReadToEnd();

                if (response.Contains("No match for domain"))
                {
                    IsAvailable = true;
                }
                else
                {
                    IsAvailable = false;
                }
            }
        }
    }
}