using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Web_Studio.Editor;
using Web_Studio.Events;
using Web_Studio.Localization;
using Web_Studio.Models;
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

            //Manage events
            EventSystem.Subscribe<FontSizeChangedEvent>(ManageChangeFont);
            EventSystem.Subscribe<ShowLineNumbersEvent>(ManageChangeShowLineNumbers);
            EventSystem.Subscribe<ClosedDocumentEvent>(ManageDocumentClosed);
            EventSystem.Subscribe<ChangedProjectEvent>(ManageChangeProject);
        }

        /// <summary>
        ///     Path to the loaded project
        /// </summary>
        public string ProjectPath
        {
            get { return _projectPath; }
            set { SetProperty(ref _projectPath, value); }
        }

        /// <summary>
        ///     Collection of Documents, editor tabs
        /// </summary>
        public ObservableCollection<EditorViewModel> Documents { get; }

        private void ManageChangeProject(ChangedProjectEvent obj)
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
        private void ManageChangeShowLineNumbers(ShowLineNumbersEvent obj)
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
        private void ManageChangeFont(FontSizeChangedEvent obj)
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

                var editorViewModel = Documents.Where(doc => doc.ToolTip == SelectedItemPath);
                if (!editorViewModel.Any())
                {
                    Documents.Add(new EditorViewModel(SelectedItemName, SelectedItemPath, EditorShowLineNumbers,
                        EditorLinkTextForegroundBrush, EditorFontSize));
                }
                else
                {
                    editorViewModel.First().IsSelected = true;
                }
            }
        }

        #endregion
    }
}