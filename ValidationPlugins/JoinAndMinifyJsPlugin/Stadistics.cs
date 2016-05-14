using System.Collections.Generic;
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
        /// <param name="jsDictionary"></param>
        /// <returns></returns>
        public static double Ratio(string projectPath, Dictionary<string, string> jsDictionary)
        {
            double minified = 0;
            foreach (KeyValuePair<string, string> pair in jsDictionary)
            {
                minified += new FileInfo(Path.Combine(projectPath, "js", pair.Value)).Length;
            }
            return minified/TotalJsSize;
        }
    }
}