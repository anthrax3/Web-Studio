using Newtonsoft.Json;

namespace Web_Studio.Managers
{
    /// <summary>
    /// Class to manage settings of one project
    /// </summary>
    public class ProjectManager
    {
        /// <summary>
        /// Singleton pattern
        /// </summary>
        [JsonIgnore]
        public static ProjectManager Instance { get; set; } = new ProjectManager();
        /// <summary>
        /// Project full path
        /// </summary>
        [JsonIgnore]
        public string FullPath { get; set; }
        /// <summary>
        /// Name of project
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Default constructor (Singleton pattern)
        /// </summary>
        private ProjectManager()
        {
            
        }

    }
}