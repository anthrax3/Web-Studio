using System.ComponentModel.Composition;
using System.Net.Http;
using Newtonsoft.Json;
using SocialCheckInterface;

namespace TwitterCheck
{
    [Export(typeof(ISocialCheck))]
    public class TwitterCheck : ISocialCheck
    {
        /// <summary>
        /// Name of the service
        /// </summary>
        public string ServiceName { get; set; } = "Twitter";
        /// <summary>
        /// Username in the service
        /// </summary>
        public string NameInService { get; set; }
        /// <summary>
        /// is this name available
        /// </summary>
        public bool IsAvailable { get; set; }
        /// <summary>
        /// Check method
        /// </summary>
        /// <param name="name"></param>
        public void CheckAvailability(string name)
        {
            using (var client = new HttpClient())
            {
                string response = client.GetStringAsync("https://twitter.com/users/username_available?username="+name).Result;
                TwitterJson twitterJson = JsonConvert.DeserializeObject<TwitterJson>(response);
                IsAvailable = twitterJson.Valid;
                NameInService = "@" + name;
            }
        }
    }
}
