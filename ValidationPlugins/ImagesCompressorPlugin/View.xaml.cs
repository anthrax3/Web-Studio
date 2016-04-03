using System.Windows.Controls;

namespace ImagesCompressorPlugin
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
        public View(ImagesCompressor vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}