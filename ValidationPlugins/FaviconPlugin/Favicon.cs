using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using FaviconPlugin.Annotations;
using FaviconPlugin.Properties;
using HtmlAgilityPack;
using ImageMagick;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace FaviconPlugin
{
    /// <summary>
    ///     Plugin that checks and creates a favicon
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Favicon")]
    [ExportMetadata("After", "Include")]
    public class Favicon : IValidation,INotifyPropertyChanged
    {
       
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix;

        /// <summary>
        ///     Text of the drag and drop box
        /// </summary>
        public string DragAndDropText => Strings.DragAndDrop;

        private string _pathToImage;

        /// <summary>
        ///     Path to image for favicon
        /// </summary>
        public string PathToImage {
            get { return _pathToImage; }
            set
            {
                if (value == _pathToImage) return;
                _pathToImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Display info about domain property
        /// </summary>
        public string DomainName => Strings.DomainName;

        /// <summary>
        ///     Full path to root file
        /// </summary>
        public string Domain { get; set; }

        #region IValidation
        
        /// <summary>
        ///     Name of the plugin
        /// </summary>
        public string Name => Strings.Name;

        /// <summary>
        ///     Description
        /// </summary>
        public string Description => Strings.Description;

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        public ICategoryType Type { get; } = StyleType.Instance;
        
        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        public bool IsAutoFixeable { get; set; } = false;

        /// <summary>
        ///     Is enabled this plugin
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        ///     Method to validate the project with this plugin
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public List<AnalysisResult> Check(string projectPath)
        {
            List<AnalysisResult> analysisResults = new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;

            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);
                var link = document.DocumentNode.SelectSingleNode(@"//link[@rel='icon']");
                if (link == null)
                {
                    analysisResults.Add(new AnalysisResult(file, 0, Name, Strings.NotFound, ErrorType.Instance));
                }
            }

            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            if (!IsAutoFixeable || !IsEnabled) return null;
            if (PathToImage == null)
                return new List<AnalysisResult>
                {
                    new AnalysisResult("", 0, Name, Strings.DropAnImage, ErrorType.Instance)
                };
            if (Domain == null)
                return new List<AnalysisResult>
                {
                    new AnalysisResult("", 0, Name, Strings.DomainNotFound, ErrorType.Instance)
                };

            //Creates favicon
            using (var image = new MagickImage(PathToImage))
            {
                image.Format = MagickFormat.Png;
                image.Resize(32, 32);
                File.WriteAllBytes(Path.Combine(projectPath, "favicon.png"), image.ToByteArray());
            }

            //Add html reference
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var document = new HtmlDocument();
                document.OptionWriteEmptyNodes = true; //Close tags
                document.Load(file);
                var link = document.DocumentNode.SelectSingleNode(@"//link[@rel='icon']");
                if (link == null)
                {
                    var headNode = document.DocumentNode.SelectSingleNode("//head");
                    if (headNode != null)
                    {
                        //Create meta description
                        var linkTag = document.CreateElement("link");

                        linkTag.Attributes.Add("rel", "icon");
                        linkTag.Attributes.Add("type", "image/png");
                        linkTag.Attributes.Add("href", Domain + "/favicon.png");
                        //Add to head
                        headNode.AppendChild(linkTag);
                        document.Save(file);
                    }
                }
            }


            return new List<AnalysisResult> {new AnalysisResult("",0,Name,Strings.Generated,InfoType.Instance)};
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        #endregion

        /// <summary>
        /// Event of INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method of INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}