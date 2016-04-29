using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FastObservableCollection;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using TreeViewExplorerControl;
using ValidationInterface;
using ValidationInterface.MessageTypes;
using Web_Studio.Editor;
using Web_Studio.Events;
using Web_Studio.Localization;
using Web_Studio.Models;
using Web_Studio.PluginManager;
using Web_Studio.Properties;
using Web_Studio.Utils;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     ViewModel for my custom Avalon TextEditor (aka TextEditorMvvm)
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private string _projectPath;

        /// <summary>
        ///     Default constructor, it loads the values from user config.
        /// </summary>
        public MainWindowViewModel()
        {
            if (ProjectModel.Instance.FullPath != null)
            {
                ProjectPath = ProjectModel.Instance.FullPath;
            }

            //Initialize properties
            Documents = new ObservableCollection<EditorViewModel>();
            EditorShowLineNumbers = Settings.Default.EditorShowLineNumbers;
            EditorFontSize = Settings.Default.EditorFontSize;
            EditorLinkTextForegroundBrush =
                (SolidColorBrush) new BrushConverter().ConvertFrom(Settings.Default.EditorLinkTextForegroundBrush);

            OptionWindowRequest = new InteractionRequest<INotification>();
            NewProjectRequest = new InteractionRequest<INotification>();
            PluginsWindowRequest = new InteractionRequest<INotification>();
            SaveChangesInteractionRequest = new InteractionRequest<IConfirmation>();
            ItemRemovedRequest = new InteractionRequest<IConfirmation>();
            FtpClientWindowRequest = new InteractionRequest<INotification>();

            //Manage commands
            SelectedItemChangedCommand = new DelegateCommand(SelectedItemChanged);
            OpenProjectCommand = new DelegateCommand(OpenProject);
            OptionWindowCommand = new DelegateCommand(OptionWindow);
            NewProjectCommand = new DelegateCommand(NewProject);
            GenerateCommand = new DelegateCommand(Generate);
            PluginsWindowCommand = new DelegateCommand(PluginsWindow);
            FtpClientCommand = new DelegateCommand(FtpClientWindow);
            CloseProjectCommand = new DelegateCommand(CloseProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            AddFileCommand = new DelegateCommand(AddFile);
            NewFileCommand = new DelegateCommand(NewFile);
            BusyControlCancelCommand = new DelegateCommand(BusyControlCancel);
            ExplorerControlItemRemovedCommand = new DelegateCommand<INode>(ExplorerControlItemRemoved);

            //Manage events
            EventSystem.Subscribe<FontSizeChangedEvent>(ManageChangedFont);
            EventSystem.Subscribe<ShowLineNumbersEvent>(ManageChangedShowLineNumbers);
            EventSystem.Subscribe<ClosedDocumentEvent>(ManageDocumentClosed);
            EventSystem.Subscribe<ChangedProjectEvent>(ManageChangedProject);

            //Worker
            GenerationWorker = new BackgroundWorker();
            GenerationWorker.DoWork += GenerationWorkerOnDoWork;
            GenerationWorker.RunWorkerCompleted += GenerationWorkerOnRunWorkerCompleted;
            GenerationWorker.WorkerSupportsCancellation = true;
        }

    


        /// <summary>
        ///     Path to the loaded project
        /// </summary>
        public string ProjectPath
        {
            get { return _projectPath; }
            set { SetProperty(ref _projectPath, value); }
        }

       

        private void ManageChangedProject(ChangedProjectEvent obj)
        {
            ProjectPath = ProjectModel.Instance.FullPath;
        }

        #region Menu

        /// <summary>
        ///     It request the view to open the option window
        /// </summary>
        public InteractionRequest<INotification> OptionWindowRequest { get; set; }

        /// <summary>
        ///     It request the view to open the new project window
        /// </summary>
        public InteractionRequest<INotification> NewProjectRequest { get; set; }

        /// <summary>
        /// It request the view to open the plugins window
        /// </summary>
        public InteractionRequest<INotification> PluginsWindowRequest { get; set; }

        /// <summary>
        /// It request the view to open the FTP client window
        /// </summary>
        public InteractionRequest<INotification> FtpClientWindowRequest { get; set; }

        /// <summary>
        /// Add file menu command
        /// </summary>
        public DelegateCommand AddFileCommand { get; private set; }
        
        /// <summary>
        ///  Close project menu command
        /// </summary>
        public DelegateCommand CloseProjectCommand { get; private set; }

        /// <summary>
        /// Save project menu command
        /// </summary>
        public DelegateCommand SaveProjectCommand { get; private set; }

        /// <summary>
        ///     New project menu option command
        /// </summary>
        public DelegateCommand NewProjectCommand { get; private set; }

        /// <summary>
        /// New file menu option
        /// </summary>
        public DelegateCommand NewFileCommand { get; private set; }

        /// <summary>
        ///     Open project menu command
        /// </summary>
        public DelegateCommand OpenProjectCommand { get; private set; }

        /// <summary>
        ///     Option menu command
        /// </summary>
        public DelegateCommand OptionWindowCommand { get; private set; }

        /// <summary>
        ///     Plugins menu command
        /// </summary>
        public DelegateCommand PluginsWindowCommand { get; private set; }

        /// <summary>
        /// FTP client menu command
        /// </summary>
        public DelegateCommand FtpClientCommand { get; private set; }

        /// <summary>
        /// Create and open a new file
        /// </summary>
        private void NewFile()
        {
            if (ProjectPath != null)
            {
                var saveFile = new SaveFileDialog
                {
                    CheckPathExists = true,
                    InitialDirectory = Path.Combine(ProjectPath,"src"),
                    Filter = "HTML (*.html)|*.html|CSS (*.css)|*.css|JavaScript (*.js)|*.js|" + Strings.File + " (*.*)|*.*"
                };
                if (saveFile.ShowDialog() == true)
                {
                   File.WriteAllText(saveFile.FileName,String.Empty); //Create file
                   SearchOrCreateDocument(saveFile.FileName);  
                }
            }
        }
            

        /// <summary>
        /// Select a file and copy it to project src 
        /// </summary>
        private void AddFile()
        {
            if (ProjectPath != null)
            {
                var openFile = new OpenFileDialog
                {
                    Multiselect = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                if (openFile.ShowDialog() == true)
                {
                    File.Copy(openFile.FileName,Path.Combine(ProjectPath,"src",Path.GetFileName(openFile.FileName))); //Copy file
                }
            }
            
        }

        /// <summary>
        /// Manage the close project event
        /// </summary>
        private void CloseProject()
        {   
            if (ProjectPath != null) //We have a project open
            {
                var close = CanCloseProject();
                if (close)
                {
                    OrderlyCloseProject();
                }
               
            }
        }

        /// <summary>
        /// Close project orderly
        /// </summary>
        private void OrderlyCloseProject()
        {
            //Save project
            ProjectModel.Instance.Save();
            ProjectPath = null;
            Documents.Clear();
            ProjectModel.Instance.Clear();
            Results.Clear();
        }

        /// <summary>
        /// Check if we can close the project
        /// </summary>
        /// <returns></returns>
        private bool CanCloseProject()
        {
            bool close = true;
                bool fileModified = Documents.Any(document => document.EditorIsModified);

                if (fileModified)
                {
                    SaveChangesInteractionRequest.Raise(
                        new Confirmation {Title = Strings.SaveChanges, Content = Strings.SaveProjectChangesDescription},
                        c =>
                        {
                            if (!c.Confirmed)
                            {
                                close = false;

                            }
                        }

                        );
                }
           
            return close;
        } 

        /// <summary>
        /// Save project configuration
        /// </summary>
        private void SaveProject()
        {
            ProjectModel.Instance.Save();
        }

        /// <summary>
        /// Request to open save changes window
        /// </summary>
        public  InteractionRequest<IConfirmation> SaveChangesInteractionRequest { get; } 
        /// <summary>
        ///     Raise new project request
        /// </summary>
        private void NewProject()
        {
            NewProjectRequest.Raise(new Notification {Title = Strings.WizardCreateNewProject});
        }

        /// <summary>
        ///     Raise option window request
        /// </summary>
        private void OptionWindow()
        {
            OptionWindowRequest.Raise(new Notification {Title = Strings.Options});
        }

        /// <summary>
        /// Raise plugin window request
        /// </summary>
        private void PluginsWindow()
        {
            ValidationPluginsViewModel.ConfigurationUI = null;
            ValidationPluginsViewModel.Plugins = null;
            ValidationPluginsViewModel.Plugins = ValidationPluginManager.Plugins;
            PluginsWindowRequest.Raise( new Notification {Title = "Plugins"});
        }

        /// <summary>
        /// Raise Ftp client window request
        /// </summary>
        private void FtpClientWindow()
        {
            FtpClientWindowRequest.Raise(new Notification {Title = Strings.FtpClient});
        }

        /// <summary>
        ///     Display a dialog to select a project
        /// </summary>
        private void OpenProject()
        {
            if (ProjectPath != null) //If you have an open project
            {
                var close = CanCloseProject();
                if (close)
                {
                    OrderlyCloseProject();
                }
                else
                {
                    return; //Can not close
                }
            }

            //Config
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Web Studio (*.ws)|*.ws"
            };

            if (openFile.ShowDialog() == true)
            {
                ProjectModel.Open(openFile.FileName);
                ProjectPath = ProjectModel.Instance.FullPath;
            }
        }

        #endregion

        #region Editor

        /// <summary>
        ///     Collection of Documents, editor tabs
        /// </summary>
        public ObservableCollection<EditorViewModel> Documents { get; }

        private int _editorFontSize;
        private Brush _editorLinkTextForegroundBrush;
        private bool _editorShowLineNumbers;

        private EditorViewModel _activeDocument;
        /// <summary>
        /// Active document
        /// </summary>
        public EditorViewModel ActiveDocument
        {
            get { return _activeDocument; }
            set { SetProperty(ref _activeDocument, value); }
        }

        /// <summary>
        ///     Enable to show line numbers in editor
        /// </summary>
        public bool EditorShowLineNumbers
        {
            get { return _editorShowLineNumbers; }
            set { SetProperty(ref _editorShowLineNumbers, value); }
        }


        /// <summary>
        ///     Color for links
        /// </summary>
        public Brush EditorLinkTextForegroundBrush
        {
            get { return _editorLinkTextForegroundBrush; }
            set { SetProperty(ref _editorLinkTextForegroundBrush, value); }
        }


        /// <summary>
        ///     Font size in editor
        /// </summary>
        public int EditorFontSize
        {
            get { return _editorFontSize; }
            set { SetProperty(ref _editorFontSize, value); }
        }

        /// <summary>
        ///     Remove closed document
        /// </summary>
        /// <param name="obj"></param>
        private void ManageDocumentClosed(ClosedDocumentEvent obj)
        {
            Documents.Remove(obj.ClosedDocument);
        }

        /// <summary>
        ///     Method to manage the chage show line numbers event
        /// </summary>
        /// <param name="obj"></param>
        private void ManageChangedShowLineNumbers(ShowLineNumbersEvent obj)
        {
            EditorShowLineNumbers = obj.ShowLineNumbers;
            foreach (var editorViewModel in Documents) //Update all editors
            {
                editorViewModel.EditorShowLineNumbers = EditorShowLineNumbers;
            }
        }

        /// <summary>
        ///     Method to manage the change font event
        /// </summary>
        /// <param name="obj"></param>
        private void ManageChangedFont(FontSizeChangedEvent obj)
        {
            EditorFontSize = obj.FontSize;

            foreach (var editorViewModel in Documents)
            {
                editorViewModel.EditorFontSize = EditorFontSize;
            }
        }

        #endregion

        #region Explorer control

        /// <summary>
        ///     Command to manage removed item changed
        /// </summary>
        public DelegateCommand<INode> ExplorerControlItemRemovedCommand { get; private set; }

        /// <summary>
        /// Request to show the removed item window
        /// </summary>
        public InteractionRequest<IConfirmation> ItemRemovedRequest { get; }

        /// <summary>
        /// Method to manage the remove command, ask for confirmation, and close removed documents
        /// </summary>
        /// <param name="node"></param>
        private void ExplorerControlItemRemoved(INode node)
        {
            if (node is FileNode) //Is a File
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    var contenTextBox = new TextBlock
                    {
                        Text = Strings.RemoveFileDescription,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ItemRemovedRequest.Raise(
                        new Confirmation {Content = contenTextBox, Title = Strings.RemoveFolderTitle},
                        c =>
                        {
                            if (c.Confirmed)
                            {
                                try
                                {
                                    File.Delete(node.FullPath);
                                    Documents.Remove(Documents.FirstOrDefault(t => t.ToolTip == node.FullPath));
                                    //Remove document
                                }
                                catch (Exception e)
                                {
                                    Telemetry.Telemetry.TelemetryClient.TrackException(e);
                                }
                            }
                        });
                });
            }
            if (node is FolderNode)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    var contenTextBox = new TextBlock
                    {
                        Text = Strings.RemoveFolderDescription,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    ItemRemovedRequest.Raise(
                        new Confirmation {Content = contenTextBox, Title = Strings.RemoveFolderTitle},
                        c =>
                        {
                            if (c.Confirmed)
                            {
                                try
                                {
                                    Directory.Delete(node.FullPath, true);
                                    var documentsToRemove = Documents.Where(t => t.ToolTip.Contains(node.FullPath));
                                    foreach (var document in documentsToRemove.ToList())
                                    {
                                        Documents.Remove(document); //Remove document
                                    }
                                }
                                catch (Exception e)
                                {
                                   Telemetry.Telemetry.TelemetryClient.TrackException(e);
                                }
                            }
                        });
                });
            }
        }

        private bool _selectedItemIsFolder;
        private string _selectedItemName;
        private string _selectedItemPath;

        /// <summary>
        ///     Name of the selected item in the explorer view
        /// </summary>
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { SetProperty(ref _selectedItemName, value); }
        }

        /// <summary>
        ///     Path to the selected item in the explorer view
        /// </summary>
        public string SelectedItemPath
        {
            get { return _selectedItemPath; }
            set { SetProperty(ref _selectedItemPath, value); }
        }

        /// <summary>
        ///     Is a folder the selected item in the explorer view
        /// </summary>
        public bool SelectedItemIsFolder
        {
            get { return _selectedItemIsFolder; }
            set { SetProperty(ref _selectedItemIsFolder, value); }
        }

        /// <summary>
        ///     Command to manage the selected item changed "event"
        /// </summary>
        public DelegateCommand SelectedItemChangedCommand { get; private set; }

        /// <summary>
        ///     Open the selected file and display it if you can do it
        /// </summary>
        private void SelectedItemChanged()
        {
            var s = Path.GetExtension(SelectedItemPath);
            if (SelectedItemIsFolder || s==null) return;
            var extension = s.ToLower();
          
            switch (extension)  //Only open these extensions
            {
                case ".html":
                case ".css":
                case ".js":
                case ".ws":
                    break;
                default:
                    return;
            }

            foreach (var doc in Documents)
            {
                doc.IsSelected = false;
            }

            EditorViewModel myEditor = SearchOrCreateDocument(SelectedItemPath);
            myEditor.IsSelected = true;
        }

        /// <summary>
        /// Search for a document if it finds it, it returns it, else it creates a new Document and return it
        /// </summary>
        /// <param name="path"></param> 
        private EditorViewModel SearchOrCreateDocument(string path)
        {
            var editorViewModel = Documents.Where(doc => doc.ToolTip == path);
            if (!editorViewModel.Any())
            {
                var nuevoNombre = path.Replace(ProjectPath+@"\", String.Empty);
                EditorViewModel myEditor = new EditorViewModel(nuevoNombre, path, EditorShowLineNumbers,
                    EditorLinkTextForegroundBrush, EditorFontSize);
                Documents.Add(myEditor);
                return myEditor;
            }
            return editorViewModel.First();
        }

        #endregion

        #region Messages

        private int _errorMessages;
        private int _warningMessages;
       

        /// <summary>
        /// Collection with the messages generated by the plugins
        /// </summary>
        public FastObservableCollection<AnalysisResult> Results { get; set; } = new FastObservableCollection<AnalysisResult>();

        /// <summary>
        /// Command to manage the generate option
        /// </summary>
        public DelegateCommand GenerateCommand { get; private set; }

        /// <summary>
        /// Generate the new project
        /// </summary>
        private void Generate()
        {
            if (ProjectPath != null && !IsGeneratingProject)
            {
                Telemetry.Telemetry.TelemetryClient.TrackEvent("Generation");
                Results.Clear();
                NumberOfRulesProcessed = 0;
                int counter = 0;
                foreach (Lazy<IValidation, IValidationMetadata> plugin in ValidationPluginManager.Plugins)
                {
                    if (plugin.Value.IsEnabled)
                    {
                        counter++;
                        if (plugin.Value.IsAutoFixeable)
                        {
                            counter += 2; //1º Fix and then Check 
                        }
                    }
                    if (plugin.Value.IsAutoFixeable) counter++;
                }
                NumberOfRules = counter; //Check and fix each plugin
                IsGeneratingProject = true;
                CopySourceToRelease();
                EventSystem.Publish(new MessageContainerVisibilityChangedEvent {IsVisible = true});  //Make visible messages container
                 
                GenerationWorker.RunWorkerAsync();
                
            }
        }

        /// <summary>
        /// Run plugins and fixes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doWorkEventArgs"></param>
        private void GenerationWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            string releasePath = Path.Combine(ProjectPath, "release");
            _errorMessages = 0;
            _warningMessages = 0;

            //Check loop
            foreach (Lazy<IValidation, IValidationMetadata> t in ValidationPluginManager.Plugins)
            {
                if (GenerationWorker.CancellationPending)  //Manage the cancelation event
                {
                    doWorkEventArgs.Cancel = true;
                    return;
                }
                var plugin = t.Value;
                if (plugin.IsEnabled)
                {
                    var tempResults = plugin.Check(releasePath);
                    foreach (AnalysisResult analysisResult in tempResults)
                    {
                        if (analysisResult.Type == ErrorType.Instance) _errorMessages++;
                        if (analysisResult.Type == WarningType.Instance) _warningMessages++;
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate //Update UI
                    {
                        Results.AddRange(tempResults);
                        NumberOfRulesProcessed++;
                    });
                }
            }

            //Fix loop and recheck loop
            foreach (Lazy<IValidation, IValidationMetadata> t in ValidationPluginManager.Plugins)
            {
                if (GenerationWorker.CancellationPending)  ////Manage the cancelation event
                {
                    doWorkEventArgs.Cancel = true;
                    return;
                }
                var plugin = t.Value;
                if (plugin.IsAutoFixeable && plugin.IsEnabled)
                {
                    //Fix
                    var tempResults = t.Value.Fix(releasePath);
                    foreach (AnalysisResult analysisResult in tempResults)
                    {
                        if (analysisResult.Type == ErrorType.Instance) _errorMessages++;
                        if (analysisResult.Type == WarningType.Instance) _warningMessages++;
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action) delegate //Update UI
                    {
                        Results.AddRange(tempResults);
                        NumberOfRulesProcessed++;
                    });

                    //Recheck
                    var checkResults = plugin.Check(releasePath);
                    foreach (AnalysisResult analysisResult in tempResults)
                    {
                        if (analysisResult.Type == ErrorType.Instance) _errorMessages++;
                        if (analysisResult.Type == WarningType.Instance) _warningMessages++;
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate //Update UI
                    {
                        Results.AddRange(checkResults);
                        NumberOfRulesProcessed++;
                    });
                }
            }

        }

        /// <summary>
        ///  When worker finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runWorkerCompletedEventArgs"></param>
        private void GenerationWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            IsGeneratingProject = false;
            Telemetry.Telemetry.TelemetryClient.TrackMetric("Errors",_errorMessages);  
            Telemetry.Telemetry.TelemetryClient.TrackMetric("Warnings",_warningMessages);
            Notifications.RaiseGeneratedNotification(_errorMessages,_warningMessages);
        }
        
        /// <summary>
        /// the worker for project generation
        /// </summary>
        private BackgroundWorker GenerationWorker { get; set; }

        /// <summary>
        /// Method to copy all files in source to release
        /// </summary>
        private void CopySourceToRelease()
        {
            string srcPath = Path.Combine(ProjectPath, "src");
            string releasePath = Path.Combine(ProjectPath, "release");

            if (Directory.Exists(releasePath))     //Remove old release
            {
                Directory.Delete(releasePath,true);
            }
            Directory.CreateDirectory(releasePath);

            foreach (string dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories)) //Crete all directories
            {
                Directory.CreateDirectory(dirPath.Replace(srcPath, releasePath));
            }

            foreach (string newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(srcPath, releasePath), true); 
            } 
        }

        private AnalysisResult _messageSelected;
        /// <summary>
        /// Selected message in message list control
        /// </summary>
        public AnalysisResult MessageSelected
        {
            get { return _messageSelected; }
            set
            {
                SetProperty(ref _messageSelected, value);
                if (_messageSelected!= null && _messageSelected.File != "" && File.Exists(_messageSelected.File))  //Project message
                {
                    GoToMessageLine();  
                }
            }
        }

        /// <summary>
        /// When a message is selected, open the document of the message and go to the line
        /// </summary>
        private void GoToMessageLine()
        {
            foreach (var doc in Documents)     //put select property to false
            {
                doc.IsSelected = false;
            }
            var myEditor = SearchOrCreateDocument(MessageSelected.File);
            myEditor.IsSelected = true;
            myEditor.ScrollToLine = MessageSelected.Line;
        }

        #endregion

        #region BusyControl
        private bool _isGeneratingProject;
        /// <summary>
        /// True if we are generating the project => enable busycontrol
        /// </summary>
        public bool IsGeneratingProject
        {
            get { return _isGeneratingProject; }
            set { SetProperty(ref _isGeneratingProject, value); }
        }

        private int _numberOfRules;
        /// <summary>
        /// Total number of rules to process
        /// </summary>
        public int NumberOfRules
        {
            get { return _numberOfRules; }
            set { SetProperty(ref _numberOfRules, value); }
        }

        private int _numberOfRulesProcessed;
        /// <summary>
        /// Number of rules that we have already processed
        /// </summary>
        public int NumberOfRulesProcessed
        {
            get { return _numberOfRulesProcessed; }
            set { SetProperty(ref _numberOfRulesProcessed, value); }
        }

        /// <summary>
        /// Manage the cancel button
        /// </summary>
        public DelegateCommand BusyControlCancelCommand { get; private set; }

        /// <summary>
        /// Cancel the project generation
        /// </summary>
        private void BusyControlCancel()
        {
          GenerationWorker.CancelAsync();
          Telemetry.Telemetry.TelemetryClient.TrackEvent("Cancel Generation");    
        }


        #endregion
    }
}