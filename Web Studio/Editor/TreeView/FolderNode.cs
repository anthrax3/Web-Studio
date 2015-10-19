using System.Collections.ObjectModel;

namespace Web_Studio.Editor.TreeView
{
    /// <summary>
    ///     Use this class is the node is a folder
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
        ///     Collection for subfolders
        /// </summary>
        public ObservableCollection<INode> Nodes { get; set; }

        /// <summary>
        ///     Full path of the folder
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        ///     Name of the folder
        /// </summary>
        public string Label { get; set; }
    }
}