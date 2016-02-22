using System.Windows;

namespace Web_Studio
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExplorerMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ExplorerLayout.Show();
        }
    }
}