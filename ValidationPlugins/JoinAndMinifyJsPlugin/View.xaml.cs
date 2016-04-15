using System.Windows.Controls;

namespace JoinAndMinifyJsPlugin
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
        public View(JoinAndMinifyJsPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}