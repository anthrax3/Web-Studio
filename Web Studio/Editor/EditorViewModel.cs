using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Web_Studio.Editor.SyntaxHighlighter;
using Web_Studio.Events;
using Web_Studio.Localization;

namespace Web_Studio.Editor
{
    /// <summary>
    ///     ViewModel for the editor view
    /// </summary>
    public class EditorViewModel : BindableBase
    {
        private TextDocument _document;
        private int _editorFontSize;

        private bool _editorIsModified;
        private Brush _editorLinkTextForegroundBrush;

        private bool _editorShowLineNumbers;

        private bool _isSelected;

        private IHighlightingDefinition _syntaxHighlighting;

        private string _textoToShow;

        private string _title;

        private string _toolTip;

        private FileSystemWatcher _watcher;

        /// <summary>
        ///     Default constructor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EditorViewModel(string title, string path, bool showLineNumbers, Brush linkTextForeground, int fontSize)
        {
            //Config editor
            Title = title;
            ToolTip = path;
            IsSelected = true;
            EditorShowLineNumbers = showLineNumbers;
            EditorLinkTextForegroundBrush = linkTextForeground;
            EditorFontSize = fontSize;
            CloseCommand = new DelegateCommand(CloseFile);
            SaveCommand = new DelegateCommand(SaveFile);
            SaveChangesConfirmationRequest = new InteractionRequest<IConfirmation>();
            ReloadConfirmationRequest = new InteractionRequest<IConfirmation>();
            
            //Load file
            var streamReader = File.OpenText(ToolTip);
            _document = new TextDocument(streamReader.ReadToEnd());
            streamReader.Close();

            //Load SyntaxHighlighting
            var syntaxHighlighterTool = new SyntaxHighlighterTool();
            SyntaxHighlighting = syntaxHighlighterTool.SyntaxHighlightingMode(path);


            _watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(path),
                Filter = Path.GetFileName(path),
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += WatcherOnChanged;
            // Begin watching
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Show a dialog where you can reload the file if it has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileSystemEventArgs"></param>
        private  void WatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {    
                try
                {
                    _watcher.EnableRaisingEvents = false;
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        var contenTextBox = new TextBlock
                        {
                            Text = Strings.FileHasChangedDescription,
                            Foreground = new SolidColorBrush(Colors.White)
                        };
                        ReloadConfirmationRequest.Raise(
                            new Confirmation {Content = contenTextBox, Title = Strings.FileHasChanged},
                            c =>
                            {
                                if (c.Confirmed)
                                {
                                    var streamReader = File.OpenText(ToolTip);
                                    Document = new TextDocument(streamReader.ReadToEnd());
                                    streamReader.Close();
                                }
                            });
                    });
                }

                finally
                {
                    _watcher.EnableRaisingEvents = true;
                }
        }

        /// <summary>
        /// Save file with the fileSystemWatcher disable, because we don't want a notification as "The file has changed"
        /// </summary>
        private void SaveFile()
        {
            _watcher.EnableRaisingEvents = false; //Disable fileSystemWatcher
           File.WriteAllText(ToolTip,Document.Text);
            EditorIsModified = false;
            _watcher.EnableRaisingEvents = true; //Enable fileSystemWatcher

        }

        /// <summary>
        ///     Confirmation event popup
        /// </summary>
        public InteractionRequest<IConfirmation> SaveChangesConfirmationRequest { get; }

        /// <summary>
        ///     Confirmation event popup for reload
        /// </summary>
        public InteractionRequest<IConfirmation> ReloadConfirmationRequest { get; }

        /// <summary>
        ///     the document is modified
        /// </summary>
        public bool EditorIsModified
        {
            get { return _editorIsModified; }
            set
            {
                SetProperty(ref _editorIsModified, value);
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        ///     Command to manage the close document event
        /// </summary>
        public DelegateCommand CloseCommand { get; private set; }

        /// <summary>
        ///     The Syntax highlighting configuration
        /// </summary>
        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return _syntaxHighlighting; }
            set { SetProperty(ref _syntaxHighlighting, value); }
        }

        /// <summary>
        ///     Document to show
        /// </summary>
        public TextDocument Document
        {
            get { return _document; }
            set { SetProperty(ref _document, value); }
        }

        /// <summary>
        ///     Text to show in the editor
        /// </summary>
        public string TextToShow
        {
            get { return _textoToShow; }
            set { SetProperty(ref _textoToShow, value); }
        }

        /// <summary>
        ///     Name of the file, tab title
        /// </summary>
        public string Title
        {
            get { return _title + (EditorIsModified ? "*" : String.Empty); }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        ///     The editor shows this text (in this case the file path) when you put the mouse above the title
        /// </summary>
        public string ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
        }

        /// <summary>
        ///     Selected tab in the editor
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
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

        private void CloseFile()
        {
            if (EditorIsModified)
            {
                var contenTextBox = new TextBlock
                {
                    Text = Strings.SaveChangesDescription,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                SaveChangesConfirmationRequest.Raise(
                    new Confirmation {Content = contenTextBox, Title = Strings.SaveChanges},
                    c =>
                    {
                        if (c.Confirmed) //Exit without save changes
                        {
                            EventSystem.Publish(new ClosedDocumentEvent {ClosedDocument = this});
                        }
                    });
            }
            else
            {
                EventSystem.Publish(new ClosedDocumentEvent {ClosedDocument = this});
            }
        }

        private int _scrollToLine;
        /// <summary>
        /// Move the scroll to a line
        /// </summary>
        public int ScrollToLine
        {
            get { return _scrollToLine; }
            set { SetProperty(ref _scrollToLine, value); }
        }

        /// <summary>
        /// Command to manage the save event
        /// </summary>
        public DelegateCommand SaveCommand { get; private set; }
    }
}