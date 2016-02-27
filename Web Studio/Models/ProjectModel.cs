using System;
using System.IO;
using Newtonsoft.Json;
using Web_Studio.Utils;

namespace Web_Studio.Models
{
    /// <summary>
    ///     Class to manage settings of one project
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        ///     Project full path
        /// </summary>
        [JsonIgnore] private string _fullPath;

        /// <summary>
        ///     Default constructor (Singleton pattern)
        /// </summary>
        private ProjectModel()
        {
            //Do nothing
        }

        /// <summary>
        ///     Singleton pattern
        /// </summary>
        [JsonIgnore]
        public static ProjectModel Instance { get; set; } = new ProjectModel();

        /// <summary>
        ///     Full path of project folder
        /// </summary>
        public string FullPath
        {
            get { return _fullPath; }
            set { _fullPath = value; }
        }

        /// <summary>
        ///     Name of project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Create a new project
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            try
            {
                Directory.CreateDirectory(Instance.FullPath);
                //Create project config
                Json.ObjectToFile(Instance, Path.Combine(Instance.FullPath, Instance.Name + ".ws"));


                //Create source folder
                var srcPath = Path.Combine(Instance.FullPath, "src");
                Directory.CreateDirectory(srcPath);

                //Create js folder
                Directory.CreateDirectory(Path.Combine(srcPath, "js"));

                //Create css folder
                Directory.CreateDirectory(Path.Combine(srcPath, "css"));

                //Create font folder
                Directory.CreateDirectory(Path.Combine(srcPath, "font"));

                //Create img folder
                Directory.CreateDirectory(Path.Combine(srcPath, "img"));

                //Create index file
                File.Create(Path.Combine(srcPath, "index.html"));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Open a project, load project config and enable project UI
        /// </summary>
        /// <param name="path"></param>
        public static void Open(string path)
        {
            //Load instance
            // Instance = (ProjectModel) Json.FileToObject(Instance, path);
            // Instance.FullPath = Path.GetDirectoryName(path);
            Instance.FullPath = Path.GetDirectoryName(path);
        }
    }
}