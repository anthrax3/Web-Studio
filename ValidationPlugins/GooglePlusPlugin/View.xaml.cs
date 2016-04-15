using System.Windows.Controls;

namespace GooglePlusPlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(GooglePlusPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}