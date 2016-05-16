namespace FacebookPlugin
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
        public View(FacebookPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}