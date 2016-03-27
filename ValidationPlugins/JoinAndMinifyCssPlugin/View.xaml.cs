using System.Windows.Controls;

namespace JoinAndMinifyCssPlugin
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
        public View(JoinAndMinifyCssPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}