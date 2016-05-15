using FastObservableCollection;
using Prism.Mvvm;

namespace Web_Studio.ViewModels
{
    /// <summary>
    /// About View Model
    /// </summary>
    public class AboutViewModel : BindableBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AboutViewModel()
        {
            string[] thanks =
            {
                "Visual Studio 2015", "ReSharper","AjaxMin","AlexFTPS","AvalonEdit","Extended WPF Toolkit","HTML Agility Pack","Magick .Net","MahApps","Microsoft Application Insights",
                 "Newtonsoft JSON","PRISM", "SSH.NET","WebMarkupMin","WpfLocalizeExtension","ResXManager"
            };
            Thanks.AddRange(thanks);
        }

        /// <summary>
        /// All tools and libs that they help me to create this
        /// </summary>
       public FastObservableCollection<string> Thanks { get; set; }  = new FastObservableCollection<string>();
    }
}