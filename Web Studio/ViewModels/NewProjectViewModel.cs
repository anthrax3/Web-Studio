using System;
using System.IO;
using System.Windows.Forms;
using Prism.Commands;
using Prism.Mvvm;
using Web_Studio.Models;

namespace Web_Studio.ViewModels
{
    /// <summary>
    /// New project view model
    /// </summary>
    public class NewProjectViewModel : BindableBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public NewProjectViewModel()
        {
            BrowseButton = new DelegateCommand(BrowseClick);
            WizardFinish = new DelegateCommand(WizardFinishRun);
        }

        /// <summary>
        /// Process wizard finish event
        /// </summary>
        private void WizardFinishRun()
        {
            var fullPath = Path.Combine(ProjectModel.Instance.FullPath, ProjectModel.Instance.Name);
            ProjectModel.Instance.FullPath = fullPath;
            ProjectModel.Instance.Create();
        }

        /// <summary>
        /// Browse for project folder
        /// </summary>
        private void BrowseClick()
        {
            try
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                var result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ProjectPath = folderBrowserDialog.SelectedPath;
                }
            }
            catch (Exception)
            {
                // ignored IO exception or null exception
            }
        }

        private string _projectName;
        /// <summary>
        /// Name of the project
        /// </summary>
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                SetProperty(ref _projectName, value);
                ProjectModel.Instance.Name = value;
                CheckPage1();

            }
        }

        private string _projectPath;
        /// <summary>
        /// Path to the project
        /// </summary>
        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                SetProperty(ref _projectPath, value);
                ProjectModel.Instance.FullPath = value;
                CheckPage1();
            }
        }
        private bool _nextPageEnabled;
        /// <summary>
        ///     Enable next button in wizard
        /// </summary>
        public bool NextPageEnabled
        {
            get { return _nextPageEnabled; }
            set { SetProperty(ref _nextPageEnabled, value); }
        }

        /// <summary>
        /// Check method to enable navigation from page1 to page2
        /// </summary>
        private void CheckPage1()
        {
            if (ProjectName.Length != 0 && ProjectPath.Length != 0)
            {
                NextPageEnabled = true;
            }
            else
            {
                NextPageEnabled = false;
            }
        }

        /// <summary>
        /// Browse for project's folder
        /// </summary>
        public DelegateCommand BrowseButton { get; private set; }
        /// <summary>
        /// Command to manage wizard finish event
        /// </summary>
        public DelegateCommand WizardFinish { get; private set; }

    }
}