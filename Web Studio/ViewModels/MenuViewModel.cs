using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Web_Studio.Localization;
using Web_Studio.Models.PluginManager;
using Web_Studio.Models.Project;

namespace Web_Studio.ViewModels
{
    /// <summary>
    /// ViewModel of main menu. It is a part of MainWindowViewModel
    /// </summary>
    public class MenuViewModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuViewModel()
        {
            OptionWindowRequest = new InteractionRequest<INotification>();
            NewProjectRequest = new InteractionRequest<INotification>();
            PluginsWindowRequest = new InteractionRequest<INotification>();
            SaveChangesInteractionRequest = new InteractionRequest<IConfirmation>();
            FtpClientWindowRequest = new InteractionRequest<INotification>();
            AboutWindowRequest = new InteractionRequest<INotification>();
            OpenProjectCommand = new DelegateCommand(OpenProject);
            OptionWindowCommand = new DelegateCommand(OptionWindow);
            NewProjectCommand = new DelegateCommand(NewProject);
            PluginsWindowCommand = new DelegateCommand(PluginsWindow);
            FtpClientCommand = new DelegateCommand(FtpClientWindow);
            AboutWindowCommand = new DelegateCommand(AboutWindow);
            CloseProjectCommand = new DelegateCommand(CloseProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);
            AddFileCommand = new DelegateCommand(AddFile);
            NewFileCommand = new DelegateCommand(NewFile);
        }

        /// <summary>
        ///     It request the view to open the option window
        /// </summary>
        public InteractionRequest<INotification> OptionWindowRequest { get; set; }

        /// <summary>
        ///     It request the view to open the new project window
        /// </summary>
        public InteractionRequest<INotification> NewProjectRequest { get; set; }

        /// <summary>
        /// It request the view to open the plugins window
        /// </summary>
        public InteractionRequest<INotification> PluginsWindowRequest { get; set; }

        /// <summary>
        /// It request the view to open the FTP client window
        /// </summary>
        public InteractionRequest<INotification> FtpClientWindowRequest { get; set; }

        /// <summary>
        /// It request the view to open the about window
        /// </summary>
        public InteractionRequest<INotification> AboutWindowRequest { get; set; }

        /// <summary>
        /// Add file menu command
        /// </summary>
        public DelegateCommand AddFileCommand { get; private set; }

        /// <summary>
        ///  Close project menu command
        /// </summary>
        public DelegateCommand CloseProjectCommand { get; private set; }

        /// <summary>
        /// Save project menu command
        /// </summary>
        public DelegateCommand SaveProjectCommand { get; private set; }

        /// <summary>
        ///     New project menu option command
        /// </summary>
        public DelegateCommand NewProjectCommand { get; private set; }

        /// <summary>
        /// New file menu option
        /// </summary>
        public DelegateCommand NewFileCommand { get; private set; }

        /// <summary>
        ///     Open project menu command
        /// </summary>
        public DelegateCommand OpenProjectCommand { get; private set; }

        /// <summary>
        ///     Option menu command
        /// </summary>
        public DelegateCommand OptionWindowCommand { get; private set; }

        /// <summary>
        ///     Plugins menu command
        /// </summary>
        public DelegateCommand PluginsWindowCommand { get; private set; }

        /// <summary>
        /// FTP client menu command
        /// </summary>
        public DelegateCommand FtpClientCommand { get; private set; }

        /// <summary>
        /// About window command
        /// </summary>
        public DelegateCommand AboutWindowCommand { get; private set; }

        /// <summary>
        /// Request to open save changes window
        /// </summary>
        public  InteractionRequest<IConfirmation> SaveChangesInteractionRequest { get; }

        /// <summary>
        /// Create and open a new file
        /// </summary>
        private void NewFile()
        {
            if (ProjectModel.Instance.FullPath != null)
            {
                var saveFile = new SaveFileDialog
                {
                    CheckPathExists = true,
                    InitialDirectory = Path.Combine(ProjectModel.Instance.FullPath, "src"),
                    Filter = "HTML (*.html)|*.html|CSS (*.css)|*.css|JavaScript (*.js)|*.js|" + Strings.File + " (*.*)|*.*"
                };
                if (saveFile.ShowDialog() == true)
                {
                    File.WriteAllText(saveFile.FileName,String.Empty); //Create file
                    ProjectModel.Instance.SearchOrCreateDocument(saveFile.FileName);  
                }
            }
        }

        /// <summary>
        /// Select a file and copy it to project src 
        /// </summary>
        private void AddFile()
        {
            if (ProjectModel.Instance.FullPath != null)
            {
                var openFile = new OpenFileDialog
                {
                    Multiselect = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                if (openFile.ShowDialog() == true)
                {
                    if (openFile.FileName != null)
                        File.Copy(openFile.FileName,Path.Combine(ProjectModel.Instance.FullPath, "src",Path.GetFileName(openFile.FileName))); //Copy file
                }
            }
            
        }

        /// <summary>
        /// Manage the close project event
        /// </summary>
        private void CloseProject()
        {   
            if (ProjectModel.Instance.FullPath != null) //We have a project open
            {
                var close = CanCloseProject();
                if (close)
                {
                    OrderlyCloseProject();
                }
               
            }
        }

        /// <summary>
        /// Close project orderly
        /// </summary>
        private void OrderlyCloseProject()
        {
            //Save project
            ProjectModel.Instance.Save();
            ProjectModel.Instance.Clear();
        }

        /// <summary>
        /// Check if we can close the project
        /// </summary>
        /// <returns></returns>
        private bool CanCloseProject()
        {
            bool close = true;
            bool fileModified = ProjectModel.Instance.Documents.Any(document => document.EditorIsModified);

            if (fileModified)
            {
                SaveChangesInteractionRequest.Raise(
                    new Confirmation { Title = Strings.SaveChanges, Content = Strings.SaveProjectChangesDescription },
                    c =>
                    {
                        if (!c.Confirmed)
                        {
                            close = false;

                        }
                    } 
                    );
            }

            return close;
        }

        /// <summary>
        /// Save project configuration
        /// </summary>
        private void SaveProject()
        {
            ProjectModel.Instance.Save();
        }

        /// <summary>
        ///     Raise new project request
        /// </summary>
        private void NewProject()
        {
            NewProjectRequest.Raise(new Notification {Title = Strings.WizardCreateNewProject});
        }

        /// <summary>
        ///     Raise option window request
        /// </summary>
        private void OptionWindow()
        {
            OptionWindowRequest.Raise(new Notification {Title = Strings.Options});
        }

        /// <summary>
        /// Raise plugin window request
        /// </summary>
        private void PluginsWindow()
        {
            ValidationPluginsViewModel.ConfigurationUI = null;
            ValidationPluginsViewModel.Plugins = null;
            ValidationPluginsViewModel.Plugins = ValidationPluginManager.Plugins;
            PluginsWindowRequest.Raise( new Notification {Title = "Plugins"});
        }

        /// <summary>
        /// Raise Ftp client window request
        /// </summary>
        private void FtpClientWindow()
        {
            FtpClientWindowRequest.Raise(new Notification {Title = Strings.FtpClient});
        }

        /// <summary>
        /// Raise About window request
        /// </summary>
        private void AboutWindow()
        {
            AboutWindowRequest.Raise(new Notification { Title = Strings.About });
        }

        /// <summary>
        ///     Display a dialog to select a project
        /// </summary>
        private void OpenProject()
        {
            if (ProjectModel.Instance.FullPath != null) //If you have an open project
            {
                var close = CanCloseProject();
                if (close)
                {
                    OrderlyCloseProject();
                }
                else
                {
                    return; //Can not close
                }
            }

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