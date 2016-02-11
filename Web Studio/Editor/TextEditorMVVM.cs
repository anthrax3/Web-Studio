using System.Windows;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Web_Studio.Editor
{
    /// <summary>
    ///     Custom Avalont TextEditor with MVVM pattern
    /// </summary>
    public class TextEditorMvvm : TextEditor
    {
        /// <summary>
        ///     Default constructor, it loads custom styles
        /// </summary>
        public TextEditorMvvm()
        {
            using (var reader = XmlReader.Create("Editor/SyntaxHighlighter/CSS.xshd"))
            {
                SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
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

        #endregion
    }
}