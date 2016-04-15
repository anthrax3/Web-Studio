using System.Windows.Controls;

namespace IframePlugin
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
        public View(IframePlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}