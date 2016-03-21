using System.Collections.Generic;
using System.Windows.Controls;
using ValidationInterface.CategoryTypes;

namespace ValidationInterface
{
   
    /// <summary>
    ///     Plugin validation interface
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// View showed when you select the plugin
        /// </summary>
        UserControl View { get; } 

        /// <summary>
        ///     Name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Description
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Category of the plugin
        /// </summary>
        ICategoryType Type { get; }

        /// <summary>
        ///     Results of the check method.
        /// </summary>
        List<AnalysisResult> AnalysisResults { get; }

        /// <summary>
        ///     can we automatically fix some errors?
        /// </summary>
        bool IsAutoFixeable { get; }

        /// <summary>
        ///     Is enabled this plugin
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        ///     Method to validate the project with this plugin
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        List<AnalysisResult> Check(string projectPath);

        /// <summary>
        ///     Method to fix automatically some errors
        /// </summary>
        /// <param name="projectPath"></param>
        List<AnalysisResult> Fix(string projectPath);
    }
}