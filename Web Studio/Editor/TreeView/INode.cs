namespace Web_Studio.Editor.TreeView
{
    /// <summary>
    ///     Interface for a treeview node
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///     Full path of the node
        /// </summary>
        string FullPath { get; set; }

        /// <summary>
        ///     Name of the node
        /// </summary>
        string Label { get; set; }
    }
}