using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Web_Studio.Managers;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para NewProject.xaml
    /// </summary>
    public partial class NewProject
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public NewProject()
        {
            InitializeComponent();
            Wizard.DataContext = ProjectManager.Instance;
        }

        /// <summary>
        ///     Enable next button in wizard
        /// </summary>
        public bool NextPageEnabled { get; set; }

        private void ButtonBrowser_OnClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = folderBrowserDialog.SelectedPath;
                //If we change Text from code, the event does not trigger
                ProjectManager.Instance.FullPath = tbPath.Text;
                TextBox_TextChanged(null, null);
            }
        }

        /// <summary>
        ///     Only enabled next buttom if Texbox are filled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPath.Text.Length != 0 && tbProjectName.Text.Length != 0)
            {
                IntroPage.CanSelectNextPage = true;
                NextPageEnabled = true;
            }
            else
            {
                IntroPage.CanSelectNextPage = false;
                NextPageEnabled = false;
            }
        }

        /// <summary>
        ///     Create a folder and a project config file when wizard finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wizard_Finish(object sender, RoutedEventArgs e)
        {
            var fullPath = Path.Combine(ProjectManager.Instance.FullPath, ProjectManager.Instance.Name);
            ProjectManager.Instance.FullPath = fullPath;
            ProjectManager.Instance.Create();
        }
    }
}