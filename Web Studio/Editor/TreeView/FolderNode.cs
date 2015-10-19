using System.Collections.ObjectModel;

namespace Web_Studio.Editor.TreeView
{
    public class FolderNode : INode
    {
        public FolderNode()
        {
            Nodes = new ObservableCollection<INode>();
        }

        public string FullPath { get; set; }
        public string Label { get; set; }
        public ObservableCollection<INode> Nodes { get; set; }
    }
}