using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Web_Studio.Editor.TreeView
{
    /// <summary>
    ///     Class to manage the TreeView when a project is loaded
    /// </summary>
    public class TreeViewManager
    {
        /// <summary>
        /// Nodes of the treeview
        /// </summary>
        public static ObservableCollection<INode> Nodes = new ObservableCollection<INode>();
        /// <summary>
        /// Full path of the project folder
        /// </summary>
        private static string fullPath;

        /// <summary>
        ///     Create a TreeViewManager instance, create a Collection of Nodes and update it when there is a change
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            fullPath = path;
            GenerateTree(path, Nodes);

            //Watcher system for TreeView
            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName,
                EnableRaisingEvents = true //Begin watching
            };

            // When reload the TreeView
            watcher.Changed += WatcherOnChanged;
            watcher.Created += WatcherOnChanged;
            watcher.Deleted += WatcherOnChanged;
            watcher.Renamed += WatcherOnChanged;
        }

        /// <summary>
        ///     Generate all nodes for the TreeView
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Nodes"></param>
        private static void GenerateTree(string path, ObservableCollection<INode> Nodes)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                var directoryNode = new FolderNode {FullPath = directory, Label = Path.GetFileName(directory)};
                GenerateTree(directory, directoryNode.Nodes);
                Nodes.Add(directoryNode);
            }

            //Adding files
            foreach (var file in Directory.GetFiles(path))
            {
                Nodes.Add(new FileNode {FullPath = file, Label = Path.GetFileName(file)});
            }
        }

        private static void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            //Use UI thread for view update
            Application.Current.Dispatcher.BeginInvoke((Action) (() =>
            {
                Nodes.Clear();
                GenerateTree(fullPath, Nodes);
            }));
        }
    }
}