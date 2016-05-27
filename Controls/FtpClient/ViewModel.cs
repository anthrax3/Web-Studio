using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FtpClient.Protocols;
using FtpClient.Protocols.FTP;
using FtpClient.Protocols.ItemTypes;
using FtpClient.Protocols.Messages;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Mvvm;

namespace FtpClient
{
    /// <summary>
    ///     MainWindow ViewModel
    /// </summary>
    public class ViewModel : BindableBase
    {
        private IProtocol _protocol;

        /// <summary>
        ///     Default constructor
        /// </summary>
        private ViewModel()
        {
            ConnectCommand = new DelegateCommand<PasswordBox>(Connect);
            Protocols = new ObservableCollection<string> {"SFTP", "FTP", "FTPS"};
            IsConnected = false;
            ShowSitesManagerCommand = new DelegateCommand(ShowSitesManager);

            LocalBrowserCommand = new DelegateCommand<ProtocolItem>(LocalBrowser);
            ParentFolderLocalCommand = new DelegateCommand(ParentLocalFolder);
            UploadCommand = new DelegateCommand(Upload);
            RefreshLocalCommand = new DelegateCommand(RefreshLocal);

            RemoteItems = new ObservableCollection<ProtocolItem>();
            RefreshRemoteCommand = new DelegateCommand(RefreshRemote);
            ParentFolderRemoteCommand = new DelegateCommand(ParentFolderRemote);
            RemoteBrowserCommand = new DelegateCommand<ProtocolItem>(RemoteBrowser);
            DownloadCommand = new DelegateCommand(Download);

            NewSiteCommand = new DelegateCommand(NewSite);
            DeleteSiteCommand = new DelegateCommand(DeleteSite);

            RunTasksCommand = new DelegateCommand(RunTasks);
            CleanCompletedTasksCommand = new DelegateCommand(CleanCompletedTasks);
            _tasksWorker = new BackgroundWorker();
            _tasksWorker.DoWork += TasksWorkerOnDoWork;
            _tasksWorker.RunWorkerCompleted += TasksWorkerOnRunWorkerCompleted;
            ProgressBarValue = 0;
            ProgressBarVisibility = Visibility.Hidden;
            RemoveTaskCommand = new DelegateCommand(RemoveTask);
            Tasks = new ObservableCollection<ProtocolTask>();
            CollectionViewSource.GetDefaultView(Tasks).GroupDescriptions.Add(new PropertyGroupDescription("Type"));
            //Group Property
            InitLocal();
        }

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ViewModel Instance { get; }  = new ViewModel();

        #region Local

        /// <summary>
        ///     Items selected in local explorer
        /// </summary>
        public List<ProtocolItem> LocalSelectedItems { get; set; } = new List<ProtocolItem>();

        /// <summary>
        ///     Method for get all files and folders in a folder
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="path"></param>
        public void GetLocalFilesAndFolders(ObservableCollection<ProtocolItem> destination, string path)
        {
            destination.Clear();
            var directories = Directory.GetDirectories(path);
            foreach (var entry in directories)
            {
                var di = new DirectoryInfo(entry);
                LocalItems.Add(new ProtocolItem(Path.GetFileName(entry), entry, 0, di.LastWriteTime, FolderType.Instance));
            }
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                LocalItems.Add(new ProtocolItem(Path.GetFileName(file), file, fi.Length, fi.LastWriteTime,
                    FileType.Instance));
            }
        }

        /// <summary>
        ///     Go to parent folder in explorer
        /// </summary>
        private void ParentLocalFolder()
        {
            try
            {
                var parent = Directory.GetParent(LocalPath)?.FullName;
                GetLocalFilesAndFolders(LocalItems, parent);
                LocalPath = parent;
            }
            catch (Exception)
            {
                //Go to init
                InitLocal();
            }
        }

        /// <summary>
        ///     Put the selected files in uploads messages
        /// </summary>
        private void Upload()
        {
            if (IsConnected)
            {
                foreach (var localSelectedItem in LocalSelectedItems)
                {
                    Tasks.Add(new UploadTask(localSelectedItem.FullPath,
                        RemotePath + PortablePath.PathSeparator(RemotePath) + localSelectedItem.Name,
                        localSelectedItem.Type));
                }
                LocalSelectedItems.Clear();
            }
        }

        /// <summary>
        ///     Navigate through the local file system
        /// </summary>
        private void LocalBrowser(ProtocolItem protocolItem)
        {
            if (protocolItem.Type is FolderType)
            {
                GetLocalFilesAndFolders(LocalItems, protocolItem.FullPath);
                LocalPath = protocolItem.FullPath;
            }
        }

        /// <summary>
        ///     Init Local with the System drives
        /// </summary>
        public void InitLocal()
        {
            LocalPath = null;
            LocalItems.Clear();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                LocalItems.Add(new ProtocolItem(drive.Name, drive.Name, 0, DateTime.Today, FolderType.Instance));
            }
        }

        private string _localPath;

        /// <summary>
        ///     Actual full path in local explorer
        /// </summary>
        public string LocalPath
        {
            get { return _localPath; }
            set { SetProperty(ref _localPath, value); }
        }

        /// <summary>
        ///     Refresh the explorer view
        /// </summary>
        private void RefreshLocal()
        {
            GetLocalFilesAndFolders(LocalItems, LocalPath);
        }

        /// <summary>
        ///     Command to get the parent folder view
        /// </summary>
        public DelegateCommand ParentFolderLocalCommand { get; private set; }

        /// <summary>
        ///     Command to refresh the explorer view
        /// </summary>
        public DelegateCommand RefreshLocalCommand { get; private set; }

        /// <summary>
        ///     Command to browse to subfolder
        /// </summary>
        public DelegateCommand<ProtocolItem> LocalBrowserCommand { get; private set; }

        /// <summary>
        ///     Command to upload files
        /// </summary>
        public DelegateCommand UploadCommand { get; private set; }

        /// <summary>
        ///     Files and Folders in LocalPath
        /// </summary>
        public ObservableCollection<ProtocolItem> LocalItems { get; set; } = new ObservableCollection<ProtocolItem>();

        #endregion

        #region Remote

        /// <summary>
        ///     Download all selected files
        /// </summary>
        private void Download()
        {
            if (LocalPath == null || RemoteSelectedItems == null) return;

            foreach (var remoteSelectedItem in RemoteSelectedItems)
            {
                Tasks.Add(new DownloadTask(remoteSelectedItem.FullPath,
                    LocalPath + PortablePath.PathSeparator(LocalPath) + remoteSelectedItem.Name, remoteSelectedItem.Type));
            }
            RemoteSelectedItems.Clear();
        }

        /// <summary>
        ///     Selection items in remote explorer
        /// </summary>
        public List<ProtocolItem> RemoteSelectedItems { get; set; } = new List<ProtocolItem>();

        /// <summary>
        ///     Command to browse to subfolder in remote file system
        /// </summary>
        public DelegateCommand<ProtocolItem> RemoteBrowserCommand { get; private set; }

        /// <summary>
        ///     Browse into the folder
        /// </summary>
        /// <param name="obj"></param>
        private void RemoteBrowser(ProtocolItem obj)
        {
            if (obj.Type is FolderType)
            {
                GetRemoteFilesAndFolders(RemoteItems, obj.FullPath);
                RemotePath = obj.FullPath;
            }
        }


        /// <summary>
        ///     Browse to parent folder
        /// </summary>
        private void ParentFolderRemote()
        {
            try
            {
                var parentPath = PortablePath.GetParentDirectory(RemotePath);
                GetRemoteFilesAndFolders(RemoteItems, parentPath);
                RemotePath = parentPath;
            }
            catch (Exception)
            {
                //Ignore
            }
        }

        /// <summary>
        ///     Command to browse to parent folder in remote explorer
        /// </summary>
        public DelegateCommand ParentFolderRemoteCommand { get; private set; }

        /// <summary>
        ///     Get the files and folders in a remote machine
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="path"></param>
        private void GetRemoteFilesAndFolders(ObservableCollection<ProtocolItem> destination, string path)
        {
            destination.Clear();
            destination.AddRange(_protocol.ListDirectory(path));
        }

        /// <summary>
        ///     Method to refresh the remote explorer
        /// </summary>
        private void RefreshRemote()
        {
            GetRemoteFilesAndFolders(RemoteItems, RemotePath);
        }

        /// <summary>
        ///     Command to refresh the remote explorer
        /// </summary>
        public DelegateCommand RefreshRemoteCommand { get; private set; }


        private ObservableCollection<ProtocolItem> _remoteItems;

        /// <summary>
        ///     Items in remote explorer
        /// </summary>
        public ObservableCollection<ProtocolItem> RemoteItems
        {
            get { return _remoteItems; }
            set { SetProperty(ref _remoteItems, value); }
        }

        private string _remotePath;

        /// <summary>
        ///     Remote host working directory
        /// </summary>
        public string RemotePath
        {
            get { return _remotePath; }
            set { SetProperty(ref _remotePath, value); }
        }

        /// <summary>
        ///     Command to download an item
        /// </summary>
        public DelegateCommand DownloadCommand { get; private set; }

        #endregion

        #region TopMenu

        /// <summary>
        /// Command to show the Sites Manager window
        /// </summary>
        public DelegateCommand ShowSitesManagerCommand { get; private set; }

        /// <summary>
        /// Method to show the sites manager window
        /// </summary>
        private void ShowSitesManager()
        {
            if(SelectedSite == null) SelectedSite = new Site();
            if (String.IsNullOrEmpty(SelectedSite.Server))
            {
                SelectedSite.Server = "www.web.com";
            }
            if(Sites.Count==0) //Add one if Sites are empty
            Sites.Add(SelectedSite);

            SitesManager sm = new SitesManager
            {
                DataContext = this
            };
            if (Sites.Count > 0)
            {
                SelectedSite = Sites.ElementAt(0);
            }
            MetroWindow window = new MetroWindow
            {
                Title = Strings.SitesManager,
                Content = sm,
                Height = 300,
                Width = 400,
                TitleCaps = false
            };
            window.ShowDialog();
            if (SelectedSite != null)
            {
                Server = SelectedSite.Server;
                User = SelectedSite.User;
                Port = SelectedSite.Port;
                ProtocolMode = SelectedSite.ProtocolMode;
            }
        }


        /// <summary>
        ///     Connect with remote host
        /// </summary>
        /// <param name="obj"></param>
        private void Connect(PasswordBox obj)
        {
            if (!IsConnected)
            {
                try
                {
                    if (ProtocolMode.Equals("SFTP"))
                    {
                        _protocol = new Sftp();
                    }
                    if (ProtocolMode.Equals("FTP"))
                    {
                        _protocol = new Ftp();
                    }
                    if (ProtocolMode.Equals("FTPS"))
                    {
                        _protocol = new Ftps();
                    }

                    IsConnected = _protocol.Connect(Server, Port, User, obj.Password);
                    RemotePath = _protocol.WorkingDirectory();

                    if (IsConnected)
                    {
                        RemoteItems = new ObservableCollection<ProtocolItem>(_protocol.ListDirectory(RemotePath));
                    }
                }
                catch (Exception)
                {
                    // We need some data
                }
            }
            else
            {
                IsConnected = !_protocol.Disconnect();
                _protocol = null;
                RemotePath = null;
                RemoteItems.Clear();
                Tasks.Clear();
            }
        }

        private bool _isConnected;

        /// <summary>
        ///     True if we are connecting with remote host
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                Status = value ? Strings.CloseConnection : Strings.Connect;
            }
        }

        /// <summary>
        ///     Protocols supported
        /// </summary>
        public ObservableCollection<string> Protocols { get; }


        private string _protocolMode;

        /// <summary>
        ///     Selected protocol
        /// </summary>
        public string ProtocolMode
        {
            get { return _protocolMode; }
            set { SetProperty(ref _protocolMode, value); }
        }

        private string _server;

        /// <summary>
        ///     Server path (IP, domain ...)
        /// </summary>
        public string Server
        {
            get { return _server; }
            set { SetProperty(ref _server, value); }
        }

        private string _user;

        /// <summary>
        ///     User for login in remote host
        /// </summary>
        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private string _port;

        /// <summary>
        ///     Port to connect to remote host
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        private string _status;

        /// <summary>
        ///     Current status
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        /// <summary>
        ///     Command to connect to remote host
        /// </summary>
        public DelegateCommand<PasswordBox> ConnectCommand { get; private set; }

        #endregion

        #region Messages

        /// <summary>
        ///     Command to remove a task
        /// </summary>
        public DelegateCommand RemoveTaskCommand { get; private set; }

        /// <summary>
        ///     Selected tasks
        /// </summary>
        public List<ProtocolTask> SelectedTasks { get; } = new List<ProtocolTask>();

        /// <summary>
        ///     Remove selected tasks
        /// </summary>
        private void RemoveTask()
        {
            foreach (var task in SelectedTasks.ToList())
            {
                Tasks.Remove(task);
            }
            SelectedTasks.Clear();
        }

        private readonly BackgroundWorker _tasksWorker;

        /// <summary>
        ///     Tasks to process
        /// </summary>
        public ObservableCollection<ProtocolTask> Tasks { get; set; }

        /// <summary>
        ///     Command to process the tasks
        /// </summary>
        public DelegateCommand RunTasksCommand { get; private set; }

        /// <summary>
        ///     Call to the background worker (Other thread)
        /// </summary>
        private void RunTasks()
        {
            if (!_tasksWorker.IsBusy)
            {
                ProgressBarValue = 0;
                ProgressBarVisibility = Visibility.Visible;
                _tasksWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        ///     Command to remove all completed tasks
        /// </summary>
        public DelegateCommand CleanCompletedTasksCommand { get; private set; }

        /// <summary>
        ///     Remove all completed tasks from Tasks
        /// </summary>
        private void CleanCompletedTasks()
        {
            var itemsToRemove = Tasks.Where(i => i.Status == Strings.Completed).ToList();
            foreach (var task in itemsToRemove)
            {
                Tasks.Remove(task);
            }
        }

        /// <summary>
        ///     Make the upload and download tasks
        /// </summary>
        private void TasksWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            foreach (var task in Tasks)
            {
                if (task.Status == Strings.Completed) continue;
                task.Process(_protocol);
                ProgressBarValue++;
            }
        }

        private int _progressBarValue;

        /// <summary>
        ///     number of tasks already processed
        /// </summary>
        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set { SetProperty(ref _progressBarValue, value); }
        }

        private Visibility _progressBarVisibility;

        /// <summary>
        ///     Visibility of progress bar.
        /// </summary>
        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set { SetProperty(ref _progressBarVisibility, value); }
        }

        /// <summary>
        ///     When all tasks are processed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runWorkerCompletedEventArgs"></param>
        private void TasksWorkerOnRunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            ProgressBarVisibility = Visibility.Hidden;
        }

        #endregion

        #region SitesManager
        /// <summary>
        /// Sites availables
        /// </summary>
        public static ObservableCollection<Site> Sites { get; set; } = new ObservableCollection<Site>();

        /// <summary>
        /// Command to create a new site
        /// </summary>
        public DelegateCommand NewSiteCommand { get; private set; }

        /// <summary>
        /// Command to delete the selected site
        /// </summary>
        public DelegateCommand DeleteSiteCommand { get; private set; }

        /// <summary>
        /// Method to delete the selected site
        /// </summary>
        private void DeleteSite()
        {
            if(SelectedSite!=null)
            Sites.Remove(SelectedSite);
        }

        /// <summary>
        /// Method to create a new site
        /// </summary>
        private void NewSite()
        {
            Sites.Add(new Site
            {
                Server = "www.web.com"
            });
        }

        private Site _selectedSite = new Site();

        /// <summary>
        /// Selected site
        /// </summary> 
        public Site SelectedSite {
            get { return _selectedSite; }
            set
            {
                _selectedSite = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}