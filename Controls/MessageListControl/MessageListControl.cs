﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FastObservableCollection;
using MessageListControl.Annotations;
using MessageListControl.Properties;
using ValidationInterface;
using ValidationInterface.MessageTypes;

namespace MessageListControl
{
    /// <summary>
    ///     Control to display an advanced list of messages
    /// </summary>
    public class MessageListControl : Control, INotifyPropertyChanged
    {
        private readonly PropertyGroupDescription _groupDescription = new PropertyGroupDescription("PluginName");

        static MessageListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (MessageListControl),
                new FrameworkPropertyMetadata(typeof (MessageListControl)));
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        public MessageListControl()
        {
            ShowErrors = true;
            ShowInformations = true;
            ShowWarnings = true;
        }

        /// <summary>
        ///     Results to show
        /// </summary>
        public FastObservableCollection<AnalysisResult> ItemsSource
        {
            get { return (FastObservableCollection<AnalysisResult>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        ///     Handler of the event for updating the UI
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Event for updating the UI
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Stadistics

        /// <summary>
        ///     Show Error if there is only one error or error if there is more
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
        ///     Show Warning if there is only one warning and warnings if there is more
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
        ///     Show Information if there is only one information msg and informations if there is more
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
        ///     Errors number
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
        ///     Warnings number
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
        ///     Number of information messages
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
        ///     show error messages
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
        ///     Show warning messages
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
        ///     Show information messages
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
        ///     Filter message types.
        /// </summary>
        private void FilterMessages()
        {
            if (ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(ItemsSource).Filter =
                    o => //Return true if that type is going to show
                    {
                        var data = o as AnalysisResult;
                        if (data != null)
                        {
                            if (data.Type is ErrorType && ShowErrors)
                            {
                                return true;
                            }
                            if (data.Type is WarningType && ShowWarnings)
                            {
                                return true;
                            }
                            if (data.Type is InfoType && ShowInformations)
                            {
                                return true;
                            }
                        }
                        return false;
                    };
            }
        }


        /// <summary>
        ///     Calculate the number of error warning and information messages
        /// </summary>
        private void GenerateStatistics()
        {
            int errors = 0, warnings = 0, informations = 0; //Temp vars for avoid OnPropertyChanged event each time that I change the value.

            foreach (var analysisResult in ItemsSource)
            {
                if (analysisResult.Type is ErrorType) errors++;
                if (analysisResult.Type is WarningType) warnings++;
                if (analysisResult.Type is InfoType) informations++;
            }
            Errors = errors;
            Warnings = warnings;
            Informations = informations;
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Selected Item in data grid
        /// </summary>
        public AnalysisResult SelectedItem
        {
            get { return (AnalysisResult)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Register selected item dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(AnalysisResult), typeof(MessageListControl));


        /// <summary>
        ///     Item Source property, Collection of Results
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof (FastObservableCollection<AnalysisResult>),
                typeof (MessageListControl),
                new FrameworkPropertyMetadata(null, ResultsChanged));

        /// <summary>
        ///     Results changed handler
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
                    var myCollection = (INotifyCollectionChanged) e.OldValue;
                    myCollection.CollectionChanged -= messageListControl.OnItemsSourceCollectionChanged;
                        //unsubscribe to collection changed event
                }
                if (e.NewValue != null)
                {
                    var collection = (FastObservableCollection<AnalysisResult>) e.NewValue;
                    CollectionViewSource.GetDefaultView(collection)
                        .GroupDescriptions.Add(messageListControl._groupDescription); //Group Property
                    messageListControl.ItemsSource = collection;
                    var myCollection = (INotifyCollectionChanged) e.NewValue;
                    myCollection.CollectionChanged += messageListControl.OnItemsSourceCollectionChanged;
                        //subscribe to collection changed event
                }
            }
        }

        /// <summary>
        ///     Handler for collection changed event of Items Source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerateStatistics();
        }

        #endregion


        /// <summary>
        /// Refresh the User Interface for example because we changed the language
        /// </summary>
        public void RefreshUI()
        {
            OnPropertyChanged(nameof(ErrorText));
            OnPropertyChanged(nameof(WarningText));
            OnPropertyChanged(nameof(InformationText));  
        }
    }
}