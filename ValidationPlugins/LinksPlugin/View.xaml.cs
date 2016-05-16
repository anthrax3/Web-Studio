namespace LinksPlugin
{
    /// <summary>
    ///     Code Behind View.xaml
    /// </summary>
    public partial class View 
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(LinksPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}