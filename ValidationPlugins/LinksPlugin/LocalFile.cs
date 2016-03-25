namespace LinksPlugin
{
    /// <summary>
    ///     Class to manage local files
    /// </summary>
    public class LocalFile
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="file"></param>
        /// <param name="projectPath"></param>
        public LocalFile(string file, string projectPath)
        {
            FullPath = file;
            IsReferenced = false;
            File = file.Replace(projectPath, string.Empty).Replace(@"\", @"/");
            if (File.StartsWith("/"))
            {
                File = File.Substring(1);
            }
        }

        /// <summary>
        ///     Relative path to local file
        /// </summary>
        public string File { get; set; }

        /// <summary>
        ///     Other file has a link to this file
        /// </summary>
        public bool IsReferenced { get; set; }

        /// <summary>
        ///     Full path to file
        /// </summary>
        public string FullPath { get; set; }
    }
}