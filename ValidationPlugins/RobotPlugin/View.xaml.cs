using System.Windows.Controls;

namespace RobotPlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        public View(RobotPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}