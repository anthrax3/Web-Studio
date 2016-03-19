using System.Windows;
using Prism.Interactivity.InteractionRequest;

namespace Web_Studio.WindowAction
{
    /// <summary>
    /// Lógica de interacción para MetroNotificationWindow.xaml
    /// </summary>
    public partial class MetroNotificationWindow 
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MetroNotificationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets or gets the <see cref="INotification"/> shown by this window./>
        /// </summary>
        public INotification Notification
        {
            get
            {
                return this.DataContext as INotification;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
