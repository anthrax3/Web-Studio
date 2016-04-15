using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusyControl.Annotations;
using BusyControl.Properties;

namespace BusyControl
{
    /// <summary>
    ///     Control to show that a program is busy, how many steps there are, what is the current step and it allows to cancel the process
    /// </summary>
    public class BusyControl : Control, INotifyPropertyChanged
    {
        static BusyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (BusyControl),
                new FrameworkPropertyMetadata(typeof (BusyControl)));
        }

        /// <summary>
        ///     Progress in the progress bar
        /// </summary>
        public string Progress => Value + "/" + MaxValue;

       

        /// <summary>
        ///     Cancel text
        /// </summary>
        public string Cancel => Strings.Cancel;

        /// <summary>
        ///     Manage the control visibility
        /// </summary>
        public Visibility ControlVisibility => IsBusy ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Property changed event for update de GUI
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Event for update de GUI
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Refresh the user interface for example because we changed the language
        /// </summary>
        public void RefreshUI()
        {
            OnPropertyChanged(nameof(Cancel));
           
        }

        #region Dependency Properties

        /// <summary>
        ///     Cancel Command
        /// </summary>
        public ICommand CancelCommand
        {
            get { return (ICommand) GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        /// <summary>
        ///     Register the cancel command
        /// </summary>
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof (ICommand), typeof (BusyControl));
        
     
        /// <summary>
        ///     Show why the program is busy
        /// </summary>
        public string Description
        {
            get { return (string) GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        ///     Register the description property
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof (string), typeof (BusyControl), new PropertyMetadata(""));


        /// <summary>
        ///     Min Value in progress bar
        /// </summary>
        public int MinValue
        {
            get { return (int) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        ///     Register minvalue property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof (int), typeof (BusyControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinValueChanged));

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyControl busyControl = d as BusyControl;
            if (busyControl != null)
            {
                busyControl.MinValue = (int)e.NewValue;
            }
        }

        /// <summary>
        ///     Max value in progress bar
        /// </summary>
        public int MaxValue
        {
            get { return (int) GetValue(MaxValueProperty); }
            set
            {
                SetValue(MaxValueProperty, value);
                OnPropertyChanged(nameof(Progress));
            }
        }

        /// <summary>
        ///     Register maxvalue property
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof (int), typeof (BusyControl), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnMaxValueChanged));

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyControl busyControl = d as BusyControl;
            if (busyControl != null)
            {
                busyControl.MaxValue = (int)e.NewValue;
            }
        }

        /// <summary>
        ///     If the program is busy => show the control
        /// </summary>
        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set
            {
                SetValue(IsBusyProperty, value);
                OnPropertyChanged(nameof(ControlVisibility));
            }
        }

        /// <summary>
        ///     Register isBusy property
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof (bool), typeof (BusyControl), new FrameworkPropertyMetadata(true,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnIsBusyChanged));

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
             BusyControl busyControl = d as BusyControl;
            if (busyControl != null)
            {
                busyControl.IsBusy = (bool) e.NewValue;
            }

        }

        /// <summary>
        ///     Current value in progress bar
        /// </summary>
        public int Value
        {
            get { return (int) GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                OnPropertyChanged(nameof(Progress));
            }
        }

        /// <summary>
        ///     Register value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (int), typeof (BusyControl), new FrameworkPropertyMetadata(0,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyControl busyControl = d as BusyControl;
            if (busyControl != null)
            {
                busyControl.Value = (int)e.NewValue;
            }
        }

        #endregion
    }
}