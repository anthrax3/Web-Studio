using System.Windows.Controls;

namespace FacebookPlugin
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
        public View(FacebookPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}