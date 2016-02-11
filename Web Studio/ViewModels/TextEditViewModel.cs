using System.Windows.Media;
using Prism.Mvvm;
using Web_Studio.Properties;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     ViewModel for my custom Avalon TextEditor (aka TextEditorMvvm)
    /// </summary>
    public class TextEditViewModel : BindableBase
    {
        /// <summary>
        ///     Default constructor, it loads the values from user config.
        /// </summary>
        public TextEditViewModel()
        {
            EditorShowLineNumbers = Settings.Default.EditorShowLineNumbers;
            EditorFontSize = Settings.Default.EditorFontSize;
            EditorLinkTextForegroundBrush =
                (SolidColorBrush) new BrushConverter().ConvertFrom(Settings.Default.EditorLinkTextForegroundBrush);
        }

        /// <summary>
        ///     Save to Settings
        /// </summary>
        public void Save()
        {
            Settings.Default.EditorShowLineNumbers = EditorShowLineNumbers;
            Settings.Default.EditorLinkTextForegroundBrush = EditorLinkTextForegroundBrush.ToString();
            Settings.Default.EditorFontSize = EditorFontSize;
            Settings.Default.Save();
        }

        #region Properties

        /// <summary>
        ///     var for property EditorShowLineNumbers
        /// </summary>
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
        ///     var for property EditorLinkTextForegroundBrush
        /// </summary>
        private Brush _editorLinkTextForegroundBrush;

        /// <summary>
        ///     Color for links
        /// </summary>
        public Brush EditorLinkTextForegroundBrush
        {
            get { return _editorLinkTextForegroundBrush; }
            set { SetProperty(ref _editorLinkTextForegroundBrush, value); }
        }

        /// <summary>
        ///     var for property EditorFontSize
        /// </summary>
        private int _editorFontSize;

        /// <summary>
        ///     Font size in editor
        /// </summary>
        public int EditorFontSize
        {
            get { return _editorFontSize; }
            set { SetProperty(ref _editorFontSize, value); }
        }

        #endregion
    }
}