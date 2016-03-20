using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Prism.Commands;

namespace Web_Studio.Editor
{
    /// <summary>
    ///     Custom Avalont TextEditor with MVVM pattern
    /// </summary>
    public class TextEditorMvvm : TextEditor
    {
        /// <summary>
        /// Default constructor, assign new shortcuts
        /// </summary>
        public TextEditorMvvm()
        {
            TextArea.DefaultInputHandler.AddBinding(new DelegateCommand(Save), ModifierKeys.Control,Key.S,null);
            TextArea.DefaultInputHandler.AddBinding(new DelegateCommand(Close),ModifierKeys.Control, Key.W,null );
        }

        /// <summary>
        /// Method to run when press Ctrl+W
        /// </summary>
        private async void Close()
        {
            var vm = DataContext as EditorViewModel;
            if (vm != null)
            {
               await vm.CloseCommand.Execute();
            }
        }

        /// <summary>
        /// Method to run when press Ctrl+S
        /// </summary>
        private async void Save()
        {
            var vm = DataContext as EditorViewModel;
            if (vm != null)
            {
                await vm.SaveCommand.Execute();
            }
        }

        #region NewProperties
        /// <summary>
        ///     Custom LinkTextForegroundBrushProperty for XAML
        /// </summary>
        public static DependencyProperty LinkTextForegroundBrushProperty =
            DependencyProperty.Register("LinkTextForegroundBrush", typeof (Brush), typeof (TextEditorMvvm),
                // binding changed callback: set value of underlying property
                new PropertyMetadata((obj, args) =>
                {
                    var target = (TextEditorMvvm) obj;
                    target.LinkTextForegroundBrush = (Brush) args.NewValue;
                })
                );

        /// <summary>
        ///     LinkTextForegroundBrushProperty
        /// </summary>
        public Brush LinkTextForegroundBrush
        {
            get { return TextArea.TextView.LinkTextForegroundBrush; }
            set { TextArea.TextView.LinkTextForegroundBrush = value; }
        }


        /// <summary>
        /// Go to a line and select its content
        /// </summary>
        public new int ScrollToLine
        {
            get { return (int)GetValue(ScrollToLineProperty); }
            set
            {
                SetValue(ScrollToLineProperty, value);
                double visualTop = TextArea.TextView.GetVisualTopByDocumentLine(value);
                ScrollToVerticalOffset(visualTop);
                DocumentLine line = Document.GetLineByNumber(value);
                Select(line.Offset, line.Length);
            }
        }

        /// <summary>
        /// Register ScrollToLine property and manage the changed event
        /// </summary>
        public static readonly DependencyProperty ScrollToLineProperty =
            DependencyProperty.Register("ScrollToLine", typeof(int), typeof(TextEditorMvvm),
                 new PropertyMetadata((obj, args) =>
                 {
                     var target = (TextEditorMvvm)obj;
                     target.ScrollToLine = (int)args.NewValue;
                 })
                );


        #endregion
    }
}