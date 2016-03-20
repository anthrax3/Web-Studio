using ValidationInterface.Properties;

namespace ValidationInterface.CategoryTypes
{
    /// <summary>
    /// About optimization that reduces the load time of a web
    /// </summary>
    public class OptimizationType : ICategoryType
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name => Strings.Optimization;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ICategoryType Instance { get; } = new OptimizationType();
    }
}