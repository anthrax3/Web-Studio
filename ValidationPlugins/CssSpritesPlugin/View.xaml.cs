namespace CssSpritesPlugin
{
    /// <summary>
    /// Code Behind View.xaml
    /// </summary>
    public partial class View
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(CssSprites vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
