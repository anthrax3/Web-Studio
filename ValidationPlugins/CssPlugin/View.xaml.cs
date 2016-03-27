using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CssPlugin
{
    /// <summary>
    /// Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(CssPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
