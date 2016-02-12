using System.Windows;
using Microsoft.Win32;
using Web_Studio.Models;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Modal window to manage the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_OnClick(object sender, RoutedEventArgs e)
        {
            var optionWindow = new Options ();
            optionWindow.Show();
        }

        /// <summary>
        ///     Display a wizard to config a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNewProject_OnClick(object sender, RoutedEventArgs e)
        {
            var newProjectWindow = new NewProject {Owner = this};
            newProjectWindow.ShowDialog();
        }

        /// <summary>
        ///     Display a dialog to select a project
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

            if (openFile.ShowDialog() == true)
            {
                ProjectModel.Open(openFile.FileName);
            }
        }
    }
}