using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Web_Studio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            XmlReader reader = XmlReader.Create("D:\\ProyectoCSharp\\otro.xshd");
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            textEditor.TextArea.TextView.LinkTextForegroundBrush = Brushes.DeepSkyBlue;


        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            XmlReader reader = XmlReader.Create("D:\\ProyectoCSharp\\otro.xshd");
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
    }
}
