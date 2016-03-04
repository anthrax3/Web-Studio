using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MessageListControl.Annotations;
using ValidationInterface;
using FastObservableCollection;
using MessageListControl.Properties;
using WPFLocalizeExtension.Providers;

namespace MessageListControl
{
   /// <summary>
   /// Control to display an advanced list of messages
   /// </summary>
    public class MessageListControl : Control,INotifyPropertyChanged
    {
       static MessageListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageListControl), new FrameworkPropertyMetadata(typeof(MessageListControl)));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageListControl()
        {
            ShowErrors = true;
            ShowInformations = true;
            ShowWarnings = true;
        }

        #region Stadistics

        /// <summary>
        /// Show Error if there is only one error or error if there is more 
        /// </summary>
       public string ErrorText
       {
           get
           {
               if (Errors == 1)
               {
                   return Strings.Error;
               }
               return Strings.Errors;
           }
       }

        /// <summary>
        /// Show Warning if there is only one warning and warnings if there is more
        /// </summary>
        public string WarningText
        {
            get
            {
                if (Warnings == 1)
                {
                    return Strings.Warning;
                }
                return Strings.Warnings;
            }
        }

        /// <summary>
        /// Show Information if there is only one information msg and informations if there is more
        /// </summary>
        public string InformationText
        {
            get
            {
                if (Informations == 1)
                {
                    return Strings.Information;
                }
                return Strings.Informations;
            }
        }

        private int _errors;

        /// <summary>
        /// Errors number
        /// </summary>
        public int Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ErrorText));
            }
        }

        private int _warnings;

        /// <summary>
        /// Warnings number
        /// </summary>
        public int Warnings
        {
            get { return _warnings; }
            set
            {
                _warnings = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WarningText));
            }
        }

        private int _informations;

        /// <summary>
        /// Number of information messages
        /// </summary>
        public int Informations
        {
            get { return _informations; }
            set
            {
                _informations = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(InformationText));
            }
        }

        private bool _showErrors;
        /// <summary>
        /// show error messages
        /// </summary>
        public bool ShowErrors
        {
            get { return _showErrors; }
            set
            {
                _showErrors = value;
                FilterMessages();
            }
        }

        private bool _showWarnings;

        /// <summary>
        /// Show warning messages
        /// </summary>
        public bool ShowWarnings
        {
            get { return _showWarnings; }
            set
            {
                _showWarnings = value;
                FilterMessages();

            }
        }

        private bool _showInformations;
        /// <summary>
        /// Show information messages
        /// </summary>
        public bool ShowInformations
        {
            get { return _showInformations; }
            set
            {
                _showInformations = value;
                FilterMessages();

            }
        }

        /// <summary>
        /// Filter message types.
        /// </summary>
        private void FilterMessages()
        {
            if (ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(ItemsSource).Filter = o => //Return true if that type is going to show
                {
                    var data = o as AnalysisResult;
                    if (data != null)
                    {
                        string type = data.Type.Name;
                        if (type.Equals("Error") && ShowErrors)
                        {
                            return true;
                        }
                        if (type.Equals("Warning") && ShowWarnings)
                        {
                            return true;
                        }
                        if (type.Equals("Information") && ShowInformations)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }
        }


        /// <summary>
        /// Calculate the number of error warning and information messages
        /// </summary>
        private void GenerateStatistics()
        {
            int errors = 0, warnings = 0, informations = 0;

            foreach (AnalysisResult analysisResult in ItemsSource)
            {
                string type = analysisResult.Type.Name;
                switch (type)
                {
                    case "Error":
                        errors++;
                        break;
                    case "Warning":
                        warnings++;
                        break;
                    case "Information":
                        informations++;
                        break;
                }
            }
            Errors = errors;
            Warnings = warnings;
            Informations = informations;
        }
        #endregion

        /// <summary>
        /// Results to show
        /// </summary>
        public FastObservableCollection<AnalysisResult> ItemsSource
        {
            get { return (FastObservableCollection<AnalysisResult>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

       

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Results.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Item Source property, Collection of Results
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(FastObservableCollection<AnalysisResult>), typeof(MessageListControl),
                new FrameworkPropertyMetadata(null, ResultsChanged));
        /// <summary>
        /// Results changed handler
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ResultsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var messageListControl = d as MessageListControl;
            if (messageListControl != null)
            {
                if (e.OldValue != null)
                {
                    var myCollection = (INotifyCollectionChanged)e.OldValue;
                    myCollection.CollectionChanged -= messageListControl.OnItemsSourceCollectionChanged; //unsubscribe to collection changed event
                }
                if (e.NewValue != null)
                {

                    messageListControl.ItemsSource = ((FastObservableCollection<AnalysisResult>)e.NewValue);
                    var myCollection = (INotifyCollectionChanged)e.NewValue;
                    myCollection.CollectionChanged += messageListControl.OnItemsSourceCollectionChanged; //subscribe to collection changed event
                }
            }
               
        }

        /// <summary>
        /// Handler for collection changed event of Items Source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
          GenerateStatistics();
        }

        #endregion

        /// <summary>
        /// Handler of the event for updating the UI
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event for updating the UI
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
