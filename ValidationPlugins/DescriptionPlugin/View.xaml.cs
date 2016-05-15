using System.Windows.Controls;

namespace DescriptionPlugin
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
        public View(DescriptionPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}