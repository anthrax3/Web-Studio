using System.Windows.Controls;

namespace CssValidatorPlugin
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
        public View(CssValidatorPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}