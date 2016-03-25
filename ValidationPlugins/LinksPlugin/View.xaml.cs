using System.Windows.Controls;

namespace LinksPlugin
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
        public View(LinksPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}