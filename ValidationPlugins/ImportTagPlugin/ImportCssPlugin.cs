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
using ImportTagPlugin.Properties;
using ValidationInterface.MessageTypes;

namespace ImportTagPlugin
{
    /// <summary>
    ///  Class to manage the @import css 
    /// </summary>
    [Export(typeof(IValidation))]
    [ExportMetadata("Name", "ImportCss")]         
    [ExportMetadata("After", "Include")]              
    public class ImportCssPlugin : IValidation  
    {
        
        /// <summary>
        ///     Text of AutoFix for binding
        /// </summary>
        public string AutoFixText => Strings.AutoFix; 

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
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();   
            if (!IsEnabled) return analysisResults;
            Regex importRegex = new Regex("^[^//*]*@import .*;"); //import without comments
            var filesToCheck = Directory.GetFiles(projectPath, "*.css", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (importRegex.IsMatch(line))
                    {
                        analysisResults.Add(new AnalysisResult(file, i+1, Name, Strings.Found, ErrorType.Instance));
                    }
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
    }
}
