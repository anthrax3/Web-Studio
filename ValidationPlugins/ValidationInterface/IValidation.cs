using System.Collections.Generic;

namespace ValidationInterface
{
    /// <summary>
    ///     Plugin category
    /// </summary>
    public enum Category
    {
        /// <summary>
        ///     About the code itself
        /// </summary>
        Development,

        /// <summary>
        ///     About Search Engine Optimization (SEO)
        /// </summary>
        Seo,

        /// <summary>
        ///     About the style of a page or a project
        /// </summary>
        Style,

        /// <summary>
        ///     About optimization that reduces the load time of a web
        /// </summary>
        Optimization
    }

    /// <summary>
    ///     Plugin validation interface
    /// </summary>
    public interface IValidation
    {
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
        Category Type { get; }

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
        void Fix(string projectPath);
    }
}