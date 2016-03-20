using ValidationInterface.Properties;

namespace ValidationInterface.CategoryTypes
{

    /// <summary>
    /// About the code itself
    /// </summary>
    public class DevelopmentType : ICategoryType
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name => Strings.Development;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ICategoryType Instance { get; } = new DevelopmentType();
    }
}