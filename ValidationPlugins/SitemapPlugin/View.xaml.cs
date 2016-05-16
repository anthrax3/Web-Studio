namespace SitemapPlugin
{
    /// <summary>
    /// Code Behind View.xaml
    /// </summary>
    public partial class View 
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public View(SitemapPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
