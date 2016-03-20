using System.Collections.ObjectModel;

namespace TreeViewExplorerControl
{
    /// <summary>
    ///     Interface for Explorer view node
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///     FullPath to file
        /// </summary>
        string FullPath { get; set; }

        /// <summary>
        ///     Name of file
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// MLD2 image code
        /// </summary>
        string Image { get; set; }

        /// <summary>
        ///     Other nodes inside this node
        /// </summary>
        ObservableCollection<INode> Nodes { get; set; }
    }
}