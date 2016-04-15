using System.Windows.Controls;

namespace CssPlugin
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
        public View(CssPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}