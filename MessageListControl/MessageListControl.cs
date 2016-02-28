using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageListControl.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using ValidationInterface;
using ValidationInterface.MessageTypes;
using FastObservableCollection;

namespace MessageListControl
{
    /// <summary>
    /// Realice los pasos 1a o 1b y luego 2 para usar este control personalizado en un archivo XAML.
    ///
    /// Paso 1a) Usar este control personalizado en un archivo XAML existente en el proyecto actual.
    /// Agregue este atributo XmlNamespace al elemento raíz del archivo de marcado en el que 
    /// se va a utilizar:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MessageListControl"
    ///
    ///
    /// Paso 1b) Usar este control personalizado en un archivo XAML existente en otro proyecto.
    /// Agregue este atributo XmlNamespace al elemento raíz del archivo de marcado en el que 
    /// se va a utilizar:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MessageListControl;assembly=MessageListControl"
    ///
    /// Tendrá también que agregar una referencia de proyecto desde el proyecto en el que reside el archivo XAML
    /// hasta este proyecto y recompilar para evitar errores de compilación:
    ///
    ///     Haga clic con el botón secundario del mouse en el proyecto de destino en el Explorador de soluciones y seleccione
    ///     "Agregar referencia"->"Proyectos"->[seleccione este proyecto]
    ///
    ///
    /// Paso 2)
    /// Prosiga y utilice el control en el archivo XAML.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
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
        private int _errors;

        public int Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        private int _warnings;

        public int Warnings
        {
            get { return _warnings; }
            set
            {
                _warnings = value;
                OnPropertyChanged();
            }
        }

        private int _informations;

        public int Informations
        {
            get { return _informations; }
            set
            {
                _informations = value;
                OnPropertyChanged();
            }
        }

        private bool _showErrors;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
