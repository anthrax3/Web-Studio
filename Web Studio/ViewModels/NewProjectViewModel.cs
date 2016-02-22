using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using Prism.Commands;
using Prism.Mvvm;
using SocialCheckInterface;
using Web_Studio.Events;
using Web_Studio.Models;

namespace Web_Studio.ViewModels
{
    /// <summary>
    ///     New project view model
    /// </summary>
    public class NewProjectViewModel : BindableBase
    {
        /// <summary>
        ///     default constructor
        /// </summary>
        public NewProjectViewModel()
        {
            WizardFinish = new DelegateCommand(WizardFinishRun);
            BrowseButton = new DelegateCommand(BrowseRun);
            SocialCheckAvailability = new DelegateCommand(SocialCheckAvailabilityRun);
            SocialCheckItems = new ObservableCollection<ISocialCheck>();
        }

        /// <summary>
        ///     Command to manage wizard finish event
        /// </summary>
        public DelegateCommand WizardFinish { get; private set; }

        /// <summary>
        ///     Process wizard finish event
        /// </summary>
        private void WizardFinishRun()
        {
            var fullPath = Path.Combine(ProjectModel.Instance.FullPath, ProjectModel.Instance.Name);
            ProjectModel.Instance.FullPath = fullPath;
            ProjectModel.Instance.Create();
            EventSystem.Publish(new ChangedProjectEvent());
        }

        #region Page 1

        /// <summary>
        ///     Browse for project folder
        /// </summary>
        private void BrowseRun()
        {
            try
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                var result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
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
        ///     Name of the project
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
        ///     Path to the project
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

        private bool _page1IsChecked;

        /// <summary>
        ///     Enable next button in wizard from page1 to page2
        /// </summary>
        public bool Page1IsChecked
        {
            get { return _page1IsChecked; }
            set { SetProperty(ref _page1IsChecked, value); }
        }

        /// <summary>
        ///     Check method to enable navigation from page1 to page2
        /// </summary>
        private void CheckPage1()
        {
            if (ProjectName.Length != 0 && ProjectPath.Length != 0)
            {
                Page1IsChecked = true;
            }
            else
            {
                Page1IsChecked = false;
            }
        }

        /// <summary>
        ///     Browse for project's folder
        /// </summary>
        public DelegateCommand BrowseButton { get; private set; }

        #endregion

        #region Page 2

        /// <summary>
        ///     Command to manage social check button click
        /// </summary>
        public DelegateCommand SocialCheckAvailability { get; private set; }

        private void SocialCheckAvailabilityRun()
        {
            var loader = new GenericMefPluginLoader<ISocialCheck>("Plugins");
            var name = ProjectName.Replace(" ", string.Empty);
            foreach (var sc in loader.Plugins)
            {
                sc.CheckAvailability(name);
                SocialCheckItems.Add(sc);
            }
        }

        private ObservableCollection<ISocialCheck> _socialCheckItems;

        /// <summary>
        ///     Collection of plugins
        /// </summary>
        public ObservableCollection<ISocialCheck> SocialCheckItems
        {
            get { return _socialCheckItems; }
            set { SetProperty(ref _socialCheckItems, value); }
        }

        #endregion
    }
}