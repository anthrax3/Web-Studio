namespace GooglePlusPlugin
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
        public View(GooglePlusPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}