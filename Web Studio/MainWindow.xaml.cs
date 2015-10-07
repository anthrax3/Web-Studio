using System.Windows;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var reader = XmlReader.Create("SyntaxHighlighter/CSS.xshd");
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            ConfigManager.Instance.Load();
            TextEditor.DataContext = ConfigManager.Instance;
            ConfigManager.Instance.Save();
        }

        /// <summary>
        ///     Modal window to manage the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new Options {Owner = this};
            a.ShowDialog();
        }
    }
}