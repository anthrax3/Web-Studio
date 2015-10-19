using System.Collections.ObjectModel;

namespace Web_Studio.Editor.TreeView
{
    public interface INode
    {
        string FullPath { get; set; }
        string Label { get; set; }
        ObservableCollection<INode> Nodes { get; set; }
    }
}