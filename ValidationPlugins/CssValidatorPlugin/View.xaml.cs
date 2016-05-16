namespace CssValidatorPlugin
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
        public View(CssValidatorPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}