using ValidationInterface.Properties;

namespace ValidationInterface.CategoryTypes
{
    /// <summary>
    /// About the style of a page or a project
    /// </summary>
    public class StyleType : ICategoryType
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name => Strings.Style;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ICategoryType Instance { get; } = new StyleType();
    }
}