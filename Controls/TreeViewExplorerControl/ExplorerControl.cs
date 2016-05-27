using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeViewExplorerControl
{
    /// <summary>
    ///     Control with a treeview for get an explorer control
    /// </summary>
    public class ExplorerControl : Control
    {
        private TreeView _myTreeView;
        private FileSystemWatcher _watcher;

        /// <summary>
        ///     Autogenerate method for a custom template
        /// </summary>
        static ExplorerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ExplorerControl),
                new FrameworkPropertyMetadata(typeof (ExplorerControl)));
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ExplorerControl()
        {
            Nodes = new ObservableCollection<INode>();
            _watcher = CreateWatcher();
        }

        private ObservableCollection<INode> Nodes { get; }

        /// <summary>
        ///     Creat a file system watcher with settings
        /// </summary>
        /// <returns></returns>
        private FileSystemWatcher CreateWatcher()
        {
            var _watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName
            };

            _watcher.Created += WatcherOnChanged;
            _watcher.Deleted += WatcherOnChanged;
            _watcher.Renamed += WatcherOnChanged;

            // Begin watching
            _watcher.EnableRaisingEvents = false;

            return _watcher;
        }

        /// <summary>
        ///     get treeview ref
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _myTreeView = GetTemplateChild("myTreeView") as TreeView;
            if (_myTreeView != null)
            {
                _myTreeView.ItemsSource = Nodes;
                _myTreeView.SelectedItemChanged += OnSelectedItemChanged;
            }
        }

        /// <summary>
        ///     When the user selected an item, we update the SelectedItem properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedPropertyChangedEventArgs"></param>
        private void OnSelectedItemChanged(object sender,
            RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
        {
            var selectedNode = (INode) ((TreeView) sender).SelectedItem;
            if (selectedNode != null) //Check null avoid Null exception
            {
                SelectedItemName = selectedNode?.Name;
                SelectedItemPath = selectedNode?.FullPath;
                if (selectedNode is FolderNode)
                {
                    SelectedItemIsFolder = true;
                }
                else
                {
                    SelectedItemIsFolder = false;
                }
                SelectedItemChanged.Execute(null);
            }
        }

        /// <summary>
        ///     If we have a change (new folder, delete folder, renamed folder), we refresh the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                Nodes.Clear();
                GenerateTree(PathToWatch, Nodes);
            }));
        }

        /// <summary>
        ///     Create the tree
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mynodes"></param>
        private void GenerateTree(string path, ObservableCollection<INode> mynodes)
        {
            //Adding folders
            foreach (var directory in Directory.GetDirectories(path))
            {
                var directoryNode = new FolderNode {FullPath = directory, Name = Path.GetFileName(directory)};
                GenerateTree(directory, directoryNode.Nodes);
                mynodes.Add(directoryNode);
            }

            //Adding files
            foreach (var file in Directory.GetFiles(path))
            {
                mynodes.Add(new FileNode {FullPath = file, Name = Path.GetFileName(file)});
            }
        }

        #region Dependency Properties

        /// <summary>
        ///     Cancel Command
        /// </summary>
        public ICommand RemoveCommand
        {
            get { return (ICommand) GetValue(RemovedCommandProperty); }
            set { SetValue(RemovedCommandProperty, value); }
        }

        /// <summary>
        ///     Register the removed command
        /// </summary>
        public static readonly DependencyProperty RemovedCommandProperty =
            DependencyProperty.Register("RemoveCommand", typeof (ICommand), typeof (ExplorerControl));

        // We need two way binding because we need to update the property value in the vm from this control 

        /// <summary>
        ///     Dependency property to get and set the path to watch
        /// </summary>
        public static readonly DependencyProperty PathToWatchProperty = DependencyProperty.Register("PathToWatch",
            typeof (string),
            typeof (ExplorerControl),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PathToWatchChanged));

        private static void PathToWatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var explorerControl = d as ExplorerControl;
            if (explorerControl != null) explorerControl.PathToWatch = (string) e.NewValue;
        }

        /// <summary>
        ///     Dependency property for SelectedItemName
        /// </summary>
        public static readonly DependencyProperty SelectedItemNameProperty =
            DependencyProperty.Register("SelectedItemName", typeof (string), typeof (ExplorerControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedItemChanges));

        private static void SelectedItemChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Do nothing, it's readonly from source
        }

        /// <summary>
        ///     Dependency property for SelectedItemPath
        /// </summary>
        public static readonly DependencyProperty SelectedItemPathProperty =
            DependencyProperty.Register("SelectedItemPath", typeof (string), typeof (ExplorerControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedItemChanges));

        /// <summary>
        ///     Dependency property for SelectedItemIsFolder
        /// </summary>
        public static readonly DependencyProperty SelectedItemIsFolderProperty =
            DependencyProperty.Register("SelectedItemIsFolder", typeof (bool), typeof (ExplorerControl),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedItemChanges));

        /// <summary>
        ///     Property to get and set the path to watch and show in the explorer control
        /// </summary>
        public string PathToWatch
        {
            get { return (string) GetValue(PathToWatchProperty); }
            set
            {
                SetValue(PathToWatchProperty, value);
                if (value != null)
                {
                    if (_watcher == null)
                    {
                        _watcher = CreateWatcher();
                    }
                    _watcher.Path = value;
                    _watcher.EnableRaisingEvents = true;
                    Nodes.Clear();
                    GenerateTree(PathToWatch, Nodes);
                }
                else
                {
                    _watcher.Dispose(); //Free
                    _watcher = null;
                    Nodes.Clear();
                }
            }
        }

        /// <summary>
        ///     Property to get the name of the selected item
        /// </summary>
        public string SelectedItemName
        {
            get { return (string) GetValue(SelectedItemNameProperty); }
            set { SetValue(SelectedItemNameProperty, value); }
        }

        /// <summary>
        ///     Property to get the path of the selected item
        /// </summary>
        public string SelectedItemPath
        {
            get { return (string) GetValue(SelectedItemPathProperty); }
            set { SetValue(SelectedItemPathProperty, value); }
        }

        /// <summary>
        ///     Property to get if the selected item is a folder (true) or a file (false)
        /// </summary>
        public bool SelectedItemIsFolder
        {
            get { return (bool) GetValue(SelectedItemIsFolderProperty); }
            set { SetValue(SelectedItemIsFolderProperty, value); }
        }

        /// <summary>
        ///     Dependency Property to manage the selected item changed event
        /// </summary>
        public static DependencyProperty SelectedItemChangedProperty = DependencyProperty.Register(
            "SelectedItemChanged", typeof (ICommand), typeof (ExplorerControl));

        /// <summary>
        ///     Command to manage the selected item changed event
        /// </summary>
        public ICommand SelectedItemChanged
        {
            get { return (ICommand) GetValue(SelectedItemChangedProperty); }
            set { SetValue(SelectedItemChangedProperty, value); }
        }

        #endregion
    }
}