using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using Web_Studio.Managers;
using Web_Studio.Utils;

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
            var reader = XmlReader.Create("Editor/SyntaxHighlighter/CSS.xshd");
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

        /// <summary>
        /// Display a wizard to config a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNewProject_OnClick(object sender, RoutedEventArgs e)
        {
            var newProjectWindow = new NewProject { Owner = this};
            newProjectWindow.ShowDialog();

        }

        /// <summary>
        /// Display a dialog to select a project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileOpenProject_OnClick(object sender, RoutedEventArgs e)
        {
            //Config
            var openFile = new OpenFileDialog
            {
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Web Studio (*.ws)|*.ws"
            };

            if (openFile.ShowDialog()==true)
            {
                ProjectManager.Open(openFile.FileName);
            }
        }
    }
}