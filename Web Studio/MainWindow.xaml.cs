using System.Windows;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Web_Studio.Managers;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var reader = XmlReader.Create("SyntaxHighlighter/CSS.xshd");
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            ConfigManager.Instance.Load(TextEditor);
        }

        /// <summary>
        ///     Modal window to manage the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_OnClick(object sender, RoutedEventArgs e)
        {
            var optionWindow = new Options {Owner = this};
            optionWindow.ShowDialog();
        }

        private void FileNewProject_OnClick(object sender, RoutedEventArgs e)
        {
            var newProjectWindow = new NewProject { Owner = this};
            newProjectWindow.ShowDialog();

        }
    }
}