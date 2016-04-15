using System.Windows.Controls;

namespace TextRatioPlugin
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
        public View(TextRatioPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}