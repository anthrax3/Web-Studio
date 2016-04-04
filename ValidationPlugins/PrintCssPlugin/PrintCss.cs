using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using PrintCssPlugin.Properties;
using ValidationInterface.MessageTypes;

namespace PrintCssPlugin
{
    /// <summary>
    /// Class to check if you have a print style sheet 
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "PrintCss")]          
    [ExportMetadata("After", "CssValidator")]              
    public class PrintCss : IValidation  
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PrintCss()
        {
            View = new View(this);
        } 

        #region IValidation
        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl View { get; }

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
        ///     Results of the check method.
        /// </summary>
        public List<AnalysisResult> AnalysisResults { get; } = new List<AnalysisResult>();

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
            AnalysisResults.Clear();
            if (!IsEnabled) return AnalysisResults;
            var filesToCheck = Directory.GetFiles(projectPath, "*.css", SearchOption.AllDirectories);
            Regex match = new Regex("@media[ ]+print");
            int counter = 0;
            foreach (string file in filesToCheck)
            {
                var content = File.ReadAllText(file);
                if (match.IsMatch(content))
                {
                    AnalysisResults.Add(new AnalysisResult(file,0,Name,Strings.Found,InfoType.Instance));
                    counter++;
                }
            }
            if(counter==0) AnalysisResults.Add(new AnalysisResult("",0,Name,Strings.NotFound,ErrorType.Instance));

            return AnalysisResults;

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
        #endregion
    }
}
