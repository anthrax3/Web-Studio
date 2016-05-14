using System.Collections.Generic;
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
        /// <param name="cssDictionary"></param>
        /// <returns></returns>
        public static double Ratio(string projectPath, Dictionary<string, string> cssDictionary)
        {
            double minified = 0;
            foreach (KeyValuePair<string, string> pair in cssDictionary)
            {
                minified += new FileInfo(Path.Combine(projectPath,"css",pair.Value)).Length;                
            }
            return minified/TotalCssSize;
        }
    }
}