using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

/*
    ///     Realice los pasos 1a o 1b y luego 2 para usar este control personalizado en un archivo XAML.
    ///     Paso 1a) Usar este control personalizado en un archivo XAML existente en el proyecto actual.
    ///     Agregue este atributo XmlNamespace al elemento raíz del archivo de marcado en el que
    ///     se va a utilizar:
    ///     xmlns:MyNamespace="clr-namespace:TreeViewExplorerControl"
    ///     Paso 1b) Usar este control personalizado en un archivo XAML existente en otro proyecto.
    ///     Agregue este atributo XmlNamespace al elemento raíz del archivo de marcado en el que
    ///     se va a utilizar:
    ///     xmlns:MyNamespace="clr-namespace:TreeViewExplorerControl;assembly=TreeViewExplorerControl"
    ///     Tendrá también que agregar una referencia de proyecto desde el proyecto en el que reside el archivo XAML
    ///     hasta este proyecto y recompilar para evitar errores de compilación:
    ///     Haga clic con el botón secundario del mouse en el proyecto de destino en el Explorador de soluciones y seleccione
    ///     "Agregar referencia"->"Proyectos"->[seleccione este proyecto]
    ///     Paso 2)
    ///     Prosiga y utilice el control en el archivo XAML.
    ///     <MyNamespace:CustomControl1 /> */

namespace TreeViewExplorerControl
{
    /// <summary>
    ///     Control with a treeview for get an explorer control
    /// </summary>
    public class ExplorerControl : Control
    {
        private readonly FileSystemWatcher _watcher;
        private TreeView _myTreeView;

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
            _watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName
            };

            _watcher.Created += WatcherOnChanged;
            _watcher.Deleted += WatcherOnChanged;
            _watcher.Renamed += WatcherOnChanged;

            // Begin watching
            _watcher.EnableRaisingEvents = false;
        }


        private ObservableCollection<INode> Nodes { get; }

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
            DependencyProperty.Register("SelectedItemName", typeof (string), typeof (ExplorerControl));

        /// <summary>
        ///     Dependency property for SelectedItemPath
        /// </summary>
        public static readonly DependencyProperty SelectedItemPathProperty =
            DependencyProperty.Register("SelectedItemPath", typeof (string), typeof (ExplorerControl));

        /// <summary>
        ///     Dependency property for SelectedItemIsFolder
        /// </summary>
        public static readonly DependencyProperty SelectedItemIsFolderProperty =
            DependencyProperty.Register("SelectedItemIsFolder", typeof (bool), typeof (ExplorerControl));

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
                    _watcher.Path = value;
                    _watcher.EnableRaisingEvents = true;
                    Nodes.Clear();
                    GenerateTree(PathToWatch, Nodes);
                }
            }
        }

        /// <summary>
        ///     Property to get the name of the selected item
        /// </summary>
        public string SelectedItemName
        {
            get { return (string) GetValue(SelectedItemNameProperty); }
            private set { SetValue(SelectedItemNameProperty, value); }
        }

        /// <summary>
        ///     Property to get the path of the selected item
        /// </summary>
        public string SelectedItemPath
        {
            get { return (string) GetValue(SelectedItemPathProperty); }
            private set { SetValue(SelectedItemPathProperty, value); }
        }

        /// <summary>
        ///     Property to get if the selected item is a folder (true) or a file (false)
        /// </summary>
        public bool SelectedItemIsFolder
        {
            get { return (bool) GetValue(SelectedItemIsFolderProperty); }
            private set { SetValue(SelectedItemIsFolderProperty, value); }
        }

        #endregion
    }
}