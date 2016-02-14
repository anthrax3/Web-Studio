using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SocialCheckInterface;

namespace SocialTest
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<ISocialCheck> plugins; 

        public MainWindow()
        {
            InitializeComponent();
            GenericMEFPluginLoader<ISocialCheck> loader = new GenericMEFPluginLoader<ISocialCheck>("Plugins");
            plugins = loader.Plugins;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mySC.Children.RemoveRange(0,mySC.Children.Count);
                string name = ((TextBox) sender).Text;
                foreach (ISocialCheck sc in plugins)
                {
                    sc.CheckAvailability(name);
                    TextBlock tb = new TextBlock();
                    tb.Text = sc.ServiceName + " - " + sc.NameInService + " - " + sc.IsAvailable;
                    mySC.Children.Add(tb);

                }
            }
        }
    }
}
