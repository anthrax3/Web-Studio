using System.Collections.ObjectModel;

namespace Web_Studio.Editor.TreeView
{
    /// <summary>
    /// Use this class when the node is a file
    /// </summary>
    public class FileNode : INode
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FileNode()
        {
            Nodes = new ObservableCollection<INode>();
        }

        /// <summary>
        /// Full path of the file
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// Name of the file
        /// </summary>
        public string Label { get; set; }
        public ObservableCollection<INode> Nodes { get; set; }
    }
}