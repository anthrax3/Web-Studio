using System.Collections.ObjectModel;

namespace TreeViewExplorerControl
{
    /// <summary>
    ///     Node for files
    /// </summary>
    public class FileNode : INode
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public FileNode()
        {
            Nodes = new ObservableCollection<INode>();
        }

        /// <summary>
        ///     Full path to file
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        ///     File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     We need it for treeview implementation, always 0 elements
        /// </summary>
        public ObservableCollection<INode> Nodes { get; set; }
    }
}