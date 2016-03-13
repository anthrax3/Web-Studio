using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using FastObservableCollection;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using ValidationInterface;
using Web_Studio.Editor;
using Web_Studio.Events;
using Web_Studio.Localization;
using Web_Studio.Models;
using Web_Studio.PluginManager;
using Web_Studio.Properties;

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

            //Manage commands
            SelectedItemChangedCommand = new DelegateCommand(SelectedItemChanged);
            OpenProjectCommand = new DelegateCommand(OpenProject);
            OptionWindowCommand = new DelegateCommand(OptionWindow);
            NewProjectCommand = new DelegateCommand(NewProject);
            GenerateCommand = new DelegateCommand(Generate);

            //Manage events
            EventSystem.Subscribe<FontSizeChangedEvent>(ManageChangedFont);
            EventSystem.Subscribe<ShowLineNumbersEvent>(ManageChangedShowLineNumbers);
            EventSystem.Subscribe<ClosedDocumentEvent>(ManageDocumentClosed);
            EventSystem.Subscribe<ChangedProjectEvent>(ManageChangedProject);
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
        ///     New project menu option command
        /// </summary>
        public DelegateCommand NewProjectCommand { get; private set; }

        /// <summary>
        ///     Open project menu command
        /// </summary>
        public DelegateCommand OpenProjectCommand { get; private set; }

        /// <summary>
        ///     Option menu command
        /// </summary>
        public DelegateCommand OptionWindowCommand { get; private set; }

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
        ///     Display a dialog to select a project
        /// </summary>
        private void OpenProject()
        {
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
        ///     Open the selected file and display it
        /// </summary>
        private void SelectedItemChanged()
        {
            if (!SelectedItemIsFolder)
            {
                foreach (var doc in Documents)
                {
                    doc.IsSelected = false;
                }

                EditorViewModel myEditor = SearchOrCreateDocument(SelectedItemPath, SelectedItemName);
                myEditor.IsSelected = true;
            }
        }

        /// <summary>
        /// Search for a document if it finds it, it returns it, else it creates a new Document and return it
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private EditorViewModel SearchOrCreateDocument(string path, string name)
        {
            var editorViewModel = Documents.Where(doc => doc.ToolTip == path);
            if (!editorViewModel.Any())
            {
                EditorViewModel myEditor = new EditorViewModel(name, path, EditorShowLineNumbers,
                    EditorLinkTextForegroundBrush, EditorFontSize);
                Documents.Add(myEditor);
                return myEditor;
            }
            return editorViewModel.First();
        }

        #endregion

        #region Messages
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
                Results.Clear();
                NumberOfRulesProcessed = 0;
                NumberOfRules = ValidationPluginManager.Plugins.Count;
                IsGeneratingProject = true;
                CopySourceToRelease();
                string releasePath = Path.Combine(ProjectPath, "release");


                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (o, ea) =>
                {
                    for (int i = 0; i < ValidationPluginManager.Plugins.Count; i++)
                    {
                        var tempResults = ValidationPluginManager.Plugins[i].Value.Check(releasePath);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate //Update UI
                        {
                            Results.AddRange(tempResults);
                            NumberOfRulesProcessed++;
                        });
                    }
                };

                worker.RunWorkerCompleted += (sender, args) => //Finished
                {
                    IsGeneratingProject = false;
                };

                worker.RunWorkerAsync();
            }
        }
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
                if (_messageSelected.File != "")  //Project message
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
            var myEditor = SearchOrCreateDocument(MessageSelected.File, Path.GetFileName(MessageSelected.File));
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


        #endregion
    }
}