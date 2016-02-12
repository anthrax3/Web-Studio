using System;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using Web_Studio.Events;
using Web_Studio.Properties;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     ViewModel for my custom Avalon TextEditor (aka TextEditorMvvm)
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
       

        /// <summary>
        ///     Default constructor, it loads the values from user config.
        /// </summary>
        public MainWindowViewModel()
        {
            EventSystem.Subscribe<FontSizeChanged>(ChangeFont);
            EventSystem.Subscribe<ShowLineNumbersChanged>(ChangeShowLineNumbers);
            EditorShowLineNumbers = Settings.Default.EditorShowLineNumbers;
            EditorFontSize = Settings.Default.EditorFontSize;
            EditorLinkTextForegroundBrush =
                (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.Default.EditorLinkTextForegroundBrush);
        }

        private void ChangeShowLineNumbers(ShowLineNumbersChanged obj)
        {
            EditorShowLineNumbers = obj.ShowLineNumbers;
        }

        private void ChangeFont(FontSizeChanged obj)
        {
            EditorFontSize = obj.FontSize;
        }

        #region Properties

        private bool _editorShowLineNumbers;
        private Brush _editorLinkTextForegroundBrush;
        private int _editorFontSize;

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

        #endregion

    }
}