using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using Web_Studio.Editor;
using Web_Studio.Events;
using Web_Studio.Models;
using Web_Studio.Properties;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     ViewModel for my custom Avalon TextEditor (aka TextEditorMvvm)
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private int _editorFontSize;
        private Brush _editorLinkTextForegroundBrush;


        private bool _editorShowLineNumbers;

        private string _projectPath;


        private bool _selectedItemIsFolder;

        private string _selectedItemName;

        private string _selectedItemPath;


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
                (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.Default.EditorLinkTextForegroundBrush);

            //Manage commands
            SelectedItemChanged = new DelegateCommand(SelectedItemHasChanged);

            //Manage events
            EventSystem.Subscribe<FontSizeChangedEvent>(ManageChangeFont);
            EventSystem.Subscribe<ShowLineNumbersEvent>(ManageChangeShowLineNumbers);
            EventSystem.Subscribe<ClosedDocumentEvent>(ManageDocumentClosed);

        }

        private void ManageDocumentClosed(ClosedDocumentEvent obj)
        {
            Documents.Remove(obj.ClosedDocument);
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
        public DelegateCommand SelectedItemChanged { get; private set; }


        private void ManageChangeShowLineNumbers(ShowLineNumbersEvent obj)
        {
            EditorShowLineNumbers = obj.ShowLineNumbers;
            foreach (var editorViewModel in Documents) //Update all editors
            {
                editorViewModel.EditorShowLineNumbers = EditorShowLineNumbers;
            }
        }

        private void ManageChangeFont(FontSizeChangedEvent obj)
        {
            EditorFontSize = obj.FontSize;

            foreach (var editorViewModel in Documents)
            {
                editorViewModel.EditorFontSize = EditorFontSize;
            }
        }

        private void SelectedItemHasChanged()
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
    }
}