using System.IO;

namespace JoinAndMinifyCssPlugin
{
    /// <summary>
    ///     Minify Stadistics
    /// </summary>
    public class Stadistics
    {
        /// <summary>
        ///     total size of all css files before minify
        /// </summary>
        public static double TotalCssSize = 0;

        /// <summary>
        ///     Compress ratio
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static double Ratio(string projectPath)
        {
            double minified = new FileInfo(Path.Combine(projectPath, "css", "style.css")).Length;
            return minified/TotalCssSize;
        }
    }
}