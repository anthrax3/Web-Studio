using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Web_Studio
{
    /// <summary>
    /// Lógica de interacción para Options.xaml
    /// </summary>
    public partial class Options
    {
        public Options()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo("es");
           
        }

    }
}
