using System;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Web_Studio.Localization;

namespace Web_Studio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            XmlReader reader = XmlReader.Create("Temas/CSS.xshd");
            TextEditor.ShowLineNumbers = true;
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            TextEditor.TextArea.TextView.LinkTextForegroundBrush = Brushes.DeepSkyBlue;
        }

        private void Options_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var a = new Options();
            a.Show();
        }
    }
}
