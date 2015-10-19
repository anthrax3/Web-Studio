using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Windows.Threading;
using TreeView.Annotations;

namespace TreeView
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private ObservableCollection<INode> m_folders;

        public ObservableCollection<INode> Nodes
        {
            get { return m_folders; }
            set
            {
                m_folders = value;
                OnPropertyChanged("Nodes");
            }
        }
        public string path = @"C:\Users\JORGE\Desktop\hola";

        public MainWindow()
        {
            InitializeComponent();
     
            //Revisamos esta ruta
            Nodes = new ObservableCollection<INode>();
            FileSystemWatcher watcher = new FileSystemWatcher(path);

            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName ;

            watcher.Changed += WatcherOnChanged;
            watcher.Created += WatcherOnChanged;
            watcher.Deleted += WatcherOnChanged;
            watcher.Renamed += WatcherOnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            generateTree(path,Nodes);
            TreeView.ItemsSource = Nodes;
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Nodes.Clear();
                generateTree(path, Nodes);
            }));
        }

        public void generateTree(string path, ObservableCollection<INode> Nodes )
        {
           // Nodes.Add(new Node {FullPath = path,Label = System.IO.Path.GetFileName(path)});
            //Adding folders
            foreach (string directory in Directory.GetDirectories(path))
            {
                var directoryNode = new Node {FullPath = directory, Label = System.IO.Path.GetFileName(directory)};
                generateTree(directory,directoryNode.Nodes);
                Nodes.Add(directoryNode);

            }

            //Adding files
            foreach (string file in Directory.GetFiles(path))
            {
                Nodes.Add(new Node {FullPath = file,Label = System.IO.Path.GetFileName(file)});
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
   }
}

