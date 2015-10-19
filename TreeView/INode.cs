using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TreeView
{
    public interface INode
    {
         string FullPath { get; set; }
         string Label { get; set; }
        ObservableCollection<INode> Nodes { get; set; } 
    }
}