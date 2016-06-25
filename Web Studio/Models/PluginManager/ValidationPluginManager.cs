using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using ValidationInterface;
using ValidationInterface.MessageTypes;
using Web_Studio.Localization;
using Web_Studio.Models.Project;
using Web_Studio.Utils;
using Web_Studio.ViewModels;

namespace Web_Studio.Models.PluginManager
{
    /// <summary>
    ///    Class to manage the validation plugins
    /// </summary>
    public  class ValidationPluginManager
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public ValidationPluginManager(MainWindowViewModel vm)
        {
            _mainWindowViewModel = vm;

            //Worker
            GenerationWorker = new BackgroundWorker();
            GenerationWorker.DoWork += GenerationWorkerOnDoWork;
            GenerationWorker.RunWorkerCompleted += GenerationWorkerOnRunWorkerCompleted;
            GenerationWorker.WorkerSupportsCancellation = true;
        }

        private int _errorMessages;
        private int _warningMessages;
        private static ObservableCollection<Lazy<IValidation, IValidationMetadata>> _plugins;
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// List of plugins that implements IValidation interface
        /// </summary>
        public static ObservableCollection<Lazy<IValidation, IValidationMetadata>> Plugins
        {
            get
            {
                  if(_plugins==null) Load();
                return _plugins;
            }
            private set { _plugins = value; }
        }

        /// <summary>
        /// Load the validation plugins
        /// </summary>
        private  static void Load()
        {
            try
            {
                GenericMefPluginLoader<Lazy<IValidation, IValidationMetadata>> loader = new GenericMefPluginLoader<Lazy<IValidation, IValidationMetadata>>("Plugins\\Check");
                Sort(loader.Plugins);
                Telemetry.Telemetry.TelemetryClient.TrackMetric("Number of check plugins loaded",_plugins.Count);
            }
            catch (Exception e )
            {
                 Telemetry.Telemetry.TelemetryClient.TrackException(e);
            }

        }

        /// <summary>
        /// Sort the plugins to get an ordered collection
        /// </summary>
        /// <param name="plugins"></param>
        private static void Sort(IEnumerable<Lazy<IValidation, IValidationMetadata>> plugins)
        {
            Queue<Lazy<IValidation,IValidationMetadata>> queue = new Queue<Lazy<IValidation, IValidationMetadata>>();
            List<Lazy<IValidation, IValidationMetadata>> sortedList = new List<Lazy<IValidation, IValidationMetadata>>();

            //Get the firt elements and add it to the queue
            var elements = plugins.Where(p => p.Metadata.After.Equals(""));
            foreach (Lazy<IValidation, IValidationMetadata> element in elements)
            {
                queue.Enqueue(element);
            }

            while (queue.Count > 0)
            {
                //Get the first element, add to the sorted list, and enqueue the other elements that depend of this element (child elements)
                var element = queue.Dequeue();
                sortedList.Add(element);
                var childElements = plugins.Where(p => p.Metadata.After.Equals(element.Metadata.Name));
                foreach (Lazy<IValidation, IValidationMetadata> childElement in childElements)
                {
                    queue.Enqueue(childElement);
                }

            }
            Plugins = new ObservableCollection<Lazy<IValidation, IValidationMetadata>>(sortedList);
        }

        #region Background Worker

        /// <summary>
        /// the worker for project generation
        /// </summary>
        public BackgroundWorker GenerationWorker { get; }

        /// <summary>
        /// Run plugins and fixes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doWorkEventArgs"></param>
        private void GenerationWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                string releasePath = Path.Combine(ProjectModel.Instance.FullPath, "release");
                _errorMessages = 0;
                _warningMessages = 0;

                //Check loop
                foreach (Lazy<IValidation, IValidationMetadata> t in Plugins)
                {
                    if (GenerationWorker.CancellationPending)  //Manage the cancelation event
                    {
                        doWorkEventArgs.Cancel = true;
                        return;
                    }
                    var plugin = t.Value;
                    if (!plugin.IsEnabled) continue;

                    var tempResults = plugin.Check(releasePath);
                    CountMessageTypes(tempResults);
                    UpdateStatusOfGeneration(tempResults);
                }

                //Fix loop and recheck loop
                foreach (Lazy<IValidation, IValidationMetadata> t in Plugins)
                {
                    if (GenerationWorker.CancellationPending)  ////Manage the cancelation event
                    {
                        doWorkEventArgs.Cancel = true;
                        return;
                    }
                    var plugin = t.Value;

                    if (!plugin.IsAutoFixeable || !plugin.IsEnabled) continue;

                    //Fix
                    var tempResults = t.Value.Fix(releasePath);
                    CountMessageTypes(tempResults);
                    if (tempResults != null && tempResults.Count > 0) tempResults.Insert(0, new AnalysisResult("", 0, plugin.Name, Strings.FixMessages, InfoType.Instance));

                    UpdateStatusOfGeneration(tempResults);

                    //Recheck
                    var checkResults = plugin.Check(releasePath);
                    CountMessageTypes(checkResults);

                    if (checkResults != null && checkResults.Count > 0) checkResults.Insert(0, new AnalysisResult("", 0, plugin.Name, Strings.NotFixedErrors, InfoType.Instance));
                    UpdateStatusOfGeneration(checkResults);
                }
            }
            catch (Exception e)
            {
                Telemetry.Telemetry.TelemetryClient.TrackException(e);
            }
        }

        /// <summary>
        ///  Count how many messages are error messages and the number of warning messages
        /// </summary>
        /// <param name="results"></param>
        private void CountMessageTypes(List<AnalysisResult> results)
        {
            foreach (AnalysisResult analysisResult in results ?? Enumerable.Empty<AnalysisResult>())
            {
                if (analysisResult.Type == ErrorType.Instance) _errorMessages++;
                if (analysisResult.Type == WarningType.Instance) _warningMessages++;
            }
        }

        /// <summary>
        ///  When worker finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runWorkerCompletedEventArgs"></param>
        private void GenerationWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            _mainWindowViewModel.IsGeneratingProject = false;
            Telemetry.Telemetry.TelemetryClient.TrackMetric("Errors", _errorMessages);
            Telemetry.Telemetry.TelemetryClient.TrackMetric("Warnings", _warningMessages);
            Notifications.RaiseGeneratedNotification(_errorMessages, _warningMessages);
        }

        /// <summary>
        ///  This method update the UI with the actual status of the generation process
        /// </summary>
        /// <param name="results"></param>
        private void UpdateStatusOfGeneration(List<AnalysisResult> results)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate //Update UI
            {
                _mainWindowViewModel.Results.AddRange(results);
                _mainWindowViewModel.NumberOfRulesProcessed++;
            });
        } 
        #endregion
    }
}