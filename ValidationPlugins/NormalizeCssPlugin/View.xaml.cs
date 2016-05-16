using System.Windows.Controls;

namespace NormalizeCssPlugin
{
    /// <summary>
    ///     Code Behind View.xaml
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