using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using IncludePlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;
using ValidationInterface.MessageTypes;

namespace IncludePlugin
{
    /// <summary>
    ///     Plugin to include the content of a html file in other html file
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Include")]
    [ExportMetadata("After", "")]
    public class IncludePlugin : IValidation
    {
       
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
        public ICategoryType Type { get; } = DevelopmentType.Instance;

        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        public bool IsAutoFixeable { get; } = false;

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
            List<AnalysisResult> analysisResults  =  new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;

            var numIncludes = 0;
            var filesToChecks = new List<FileToCheck>();
            //Get the html files in the folder and subfolder
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in filesToCheck)
            {
                var fileToCheck = new FileToCheck(file);
                filesToChecks.Add(fileToCheck);
                numIncludes += fileToCheck.MakeInclusion(analysisResults, Name);
                fileToCheck.IncludedFile();
            }

            if (numIncludes > 0)
            {
                analysisResults.Add(new AnalysisResult
                {
                    File = "",
                    Line = 0,
                    PluginName = Name,
                    Type = InfoType.Instance,
                    Message = Strings.Realised + " " + numIncludes + " " + Strings.Inclusions
                });
            }
            RemoveIncludeFiles(filesToChecks);
            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            return null;
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }


        private void RemoveIncludeFiles(List<FileToCheck> filesToChecks)
        {
            foreach (var fileToCheck in filesToChecks.Where(fileToCheck => fileToCheck.IsIncludedFile))
            {
                try
                {
                    File.Delete(fileToCheck.FilePath);
                }
                catch (Exception)
                {
                    //  
                }
            }
        }
    }
}