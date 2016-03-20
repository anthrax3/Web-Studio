using System.Windows.Controls;

namespace HtaccessPlugin
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
        public View(HtaccessPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}