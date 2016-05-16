namespace Error404PagePlugin
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
        public View(Error404Page vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}