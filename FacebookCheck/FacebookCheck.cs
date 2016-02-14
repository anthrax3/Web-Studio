using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SocialCheckInterface;

namespace FacebookCheck
{
    [Export(typeof(ISocialCheck))]
    public class FacebookCheck : ISocialCheck
    {
        public string ServiceName { get; set; } = "Facebook";
        public string NameInService { get; set; }
        public bool IsAvailable { get; set; }

        /// <summary>
        /// FB doesn't have an API where we can check users
        /// </summary>
        /// <param name="name"></param>
        public void CheckAvailability(string name)
        {
            using (var client = new HttpClient())
            {
                NameInService = "https://wwww.facebook.com/" + name;
                // Without user agent facebook reject petition
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident / 6.0)");
                HttpStatusCode statusCode = client.GetAsync(NameInService).Result.StatusCode;
                if (statusCode == HttpStatusCode.OK)
                {
                    IsAvailable = false;
                }
                if (statusCode == HttpStatusCode.NotFound)
                {
                    IsAvailable = true;
                }
            }
        }
    }
}
