using System.Windows.Controls;

namespace HeadingPlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        ///     Default constructor, inject ViewModel
        /// </summary>
        /// <param name="vm"></param>
        public View(HeadingPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}