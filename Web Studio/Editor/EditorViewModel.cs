using System.IO;
using ICSharpCode.AvalonEdit.Document;
using Prism.Mvvm;

namespace Web_Studio.Editor
{
    /// <summary>
    /// ViewModel for the editor view
    /// </summary>
    public class EditorViewModel : BindableBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EditorViewModel(string title, string path)
        {
            Title = title;
            ToolTip = path;
            IsSelected = true;
            StreamReader streamReader = File.OpenText(ToolTip);
            _document = new TextDocument( streamReader.ReadToEnd());
            streamReader.Close();
        }

        private TextDocument _document;
        public TextDocument Document
        {
            get { return _document; }
            set { SetProperty(ref _document, value); }
        }

        private string _textoToShow;
        /// <summary>
        /// Text to show in the editor
        /// </summary>
        public string TextToShow
        {
            get { return _textoToShow; }
            set { SetProperty(ref _textoToShow, value); }
        }

        private string _title;
        /// <summary>
        /// Name of the file, tab title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _toolTip;
        /// <summary>
        /// The editor shows this text (in this case the file path) when you put the mouse above the title
        /// </summary>
        public string ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
        }

        private bool _isSelected;
        /// <summary>
        /// Selected tab in the editor
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

    }
}