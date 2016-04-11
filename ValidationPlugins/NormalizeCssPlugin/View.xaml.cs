using System.Windows.Controls;

namespace NormalizeCssPlugin
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
        public View(NormalizeCss vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}