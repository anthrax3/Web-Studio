using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TreeView
{
    public class Node : INode
    {
        public Node()
        {
            Nodes = new ObservableCollection<INode>();
        }

        public string FullPath { get; set; }
        public string Label { get; set; }
        public ObservableCollection<INode> Nodes { get; set; }
    }
}