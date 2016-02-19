using System.IO;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using Prism.Mvvm;
using Web_Studio.Editor.SyntaxHighlighter;

namespace Web_Studio.Editor
{
    /// <summary>
    ///     ViewModel for the editor view
    /// </summary>
    public class EditorViewModel : BindableBase
    {
        private TextDocument _document;
        private int _editorFontSize;
        private Brush _editorLinkTextForegroundBrush;

        private bool _editorShowLineNumbers;

        private bool _isSelected;

        private IHighlightingDefinition _syntaxHighlighting;

        private string _textoToShow;

        private string _title;

        private string _toolTip;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public EditorViewModel(string title, string path, bool showLineNumbers, Brush linkTextForeground, int fontSize)
        {
            Title = title;
            ToolTip = path;
            IsSelected = true;
            EditorShowLineNumbers = showLineNumbers;
            EditorLinkTextForegroundBrush = linkTextForeground;
            EditorFontSize = fontSize;
            var streamReader = File.OpenText(ToolTip);
            _document = new TextDocument(streamReader.ReadToEnd());
            streamReader.Close();
            var syntaxHighlighterTool = new SyntaxHighlighterTool();
            SyntaxHighlighting = syntaxHighlighterTool.SyntaxHighlightingMode(path);
        }

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
            get { return _title; }
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
    }
}