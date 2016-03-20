using System.Windows.Controls;

namespace IncludePlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        ///     Default constructor, asign ViewModel
        /// </summary>
        /// <param name="vm"></param>
        public View(IncludePlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}