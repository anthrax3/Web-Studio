using System;
using System.IO;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Web_Studio.Localization;
using Web_Studio.Properties;


namespace Web_Studio.Utils
{
    /// <summary>
    /// Permits to manage notifications
    /// </summary>
    public class Notifications
    {
        /// <summary>
        /// Raise a notification with the results after we generated the project
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="warnings"></param>
        public static void RaiseGeneratedNotification(int errors, int warnings)
        {
            // Get a toast XML template
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);
            var a = toastXml.GetXml();
            
            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode("Web Studio"));
            stringElements[1].AppendChild(toastXml.CreateTextNode(Strings.Errors+": "+errors));
            stringElements[2].AppendChild(toastXml.CreateTextNode(Strings.Warnings+": " +warnings));

            
            // Specify the absolute path to an image
            String imagePath = "file:///" + Path.GetFullPath("App.png");
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml)
            {
                ExpirationTime = DateTimeOffset.Now.AddMinutes(2)
            };

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(Resources.AppId).Show(toast);
        }  
        
    }
}