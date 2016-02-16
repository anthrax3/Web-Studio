using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SocialCheckInterface;

namespace SocialTest
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEnumerable<ISocialCheck> plugins;

        public MainWindow()
        {
            InitializeComponent();
            var loader = new GenericMEFPluginLoader<ISocialCheck>("Plugins");
            plugins = loader.Plugins;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mySC.Children.RemoveRange(0, mySC.Children.Count);
                var name = ((TextBox) sender).Text;
                foreach (var sc in plugins)
                {
                    sc.CheckAvailability(name);
                    var tb = new TextBlock();
                    tb.Text = sc.ServiceName + " - " + sc.NameInService + " - " + sc.IsAvailable;
                    mySC.Children.Add(tb);
                }
            }
        }
    }
}