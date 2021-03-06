﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using HeadingPlugin.Properties;
using ValidationInterface;
using ValidationInterface.CategoryTypes;

namespace HeadingPlugin
{
    /// <summary>
    ///     Plugin to check html heading (h1 h2 h3...)
    /// </summary>
    [Export(typeof (IValidation))]
    [ExportMetadata("Name", "Heading")]
    [ExportMetadata("After", "Include")]
    public class HeadingPlugin : IValidation
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
        public ICategoryType Type { get; } = SeoType.Instance;

        
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
            List<AnalysisResult> analysisResults  = new List<AnalysisResult>();
            if (!IsEnabled) return analysisResults;

            //Get the html files in the folder and subfolder
            var filesToCheck = Directory.GetFiles(projectPath, "*.html", SearchOption.AllDirectories);
            var models = new List<HeadingModel>();
            foreach (var file in filesToCheck)
            {
                var model = new HeadingModel(file);
                model.CheckHeadings();
                models.Add(model);
            }
            GenerateResults(models,analysisResults);
            return analysisResults;
        }

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        public List<AnalysisResult> Fix(string projectPath)
        {
            return null;
            //Do nothing
        }

        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        public UserControl GetView()
        {
            return new View(this);
        }

        private void GenerateResults(List<HeadingModel> models, List<AnalysisResult> analysisResults)
        {
            int h1 = 0, h2 = 0, h3 = 0;

            foreach (var model in models)
            {
                if (model.H1 == 0) analysisResults.Add(Messages.H1NotFound(model.File));
                if (model.H1 > 1) analysisResults.Add(Messages.ManyH1Found(model.File));
                if (model.H2 == 0) analysisResults.Add(Messages.H2NotFound(model.File));
                h1 += model.H1;
                h2 += model.H2;
                h3 += model.H3;
            }
            analysisResults.Add(Messages.TagsCount(h1, h2, h3));
        }
    }
}