using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;

namespace Web_Studio.Editor
{
    /// <summary>
    ///     Custom Avalont TextEditor with MVVM pattern
    /// </summary>
    public class TextEditorMvvm : TextEditor
    {
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

        #endregion
    }
}