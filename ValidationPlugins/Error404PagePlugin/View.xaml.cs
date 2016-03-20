using System.Windows.Controls;

namespace Error404PagePlugin
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
        public View(Error404Page vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}