using System.Collections.Generic;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.IO;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using CssSpritesPlugin.Properties;
using HtmlAgilityPack;
using ValidationInterface.MessageTypes;

namespace CssSpritesPlugin
{
    /// <summary>
    ///  This plugin checks if you have a lot of images and suggest you tu use CSS Sprites
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "CssSprites")]          
    [ExportMetadata("After", "Include")]            
    public class CssSprites : IValidation 
    { 
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
        public ICategoryType Type { get; } = OptimizationType.Instance;  

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
                document.Load(file);
                var nodes = document.DocumentNode.SelectNodes(@"//img");
                if(nodes?.Count>MaxImages)
                {
                    analysisResults.Add(new AnalysisResult(file,0,Name,Strings.MoreImages,ErrorType.Instance));
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

            return null;
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
        /// Max number of images in a page before show error message
        /// </summary>
        public int MaxImages { get; set; } = 6;

        /// <summary>
        /// MaxImages description
        /// </summary>
        public string MaxImagesText => Strings.MaxImages;
    }
}
