using System.Collections.ObjectModel;

namespace TreeViewExplorerControl
{
    /// <summary>
    ///     Class for folders
    /// </summary>
    public class FolderNode : INode
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public FolderNode()
        {
            Nodes = new ObservableCollection<INode>();
        }

        /// <summary>
        ///     Full path to folder
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        ///     Folder name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Files and folders inside this folder
        /// </summary>
        public ObservableCollection<INode> Nodes { get; set; }
    }
}