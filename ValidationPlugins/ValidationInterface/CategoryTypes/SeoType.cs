using ValidationInterface.Properties;

namespace ValidationInterface.CategoryTypes
{
    /// <summary>
    /// About Search Engine Optimization (SEO)
    /// </summary>
    public class SeoType : ICategoryType
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name => Strings.Seo;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ICategoryType Instance { get; } = new SeoType();
    }
}