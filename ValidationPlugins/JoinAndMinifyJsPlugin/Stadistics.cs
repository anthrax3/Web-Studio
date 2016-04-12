using System.IO;

namespace JoinAndMinifyJsPlugin
{
    /// <summary>
    ///     Minify Stadistics
    /// </summary>
    public class Stadistics
    {
        /// <summary>
        ///     total size of all css files before minify
        /// </summary>
        public static double TotalJsSize = 0;

        /// <summary>
        ///     Compress ratio
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static double Ratio(string projectPath)
        {
            double minified = new FileInfo(Path.Combine(projectPath, "js", "script.js")).Length;
            return minified/TotalJsSize;
        }
    }
}