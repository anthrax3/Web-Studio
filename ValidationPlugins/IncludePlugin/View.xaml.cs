namespace IncludePlugin
{
    /// <summary>
    ///     Code Behind View.xaml
    /// </summary>
    public partial class View 
    {
        /// <summary>
        ///     Default constructor, asign ViewModel
        /// </summary>
        /// <param name="vm"></param>
        public View(IncludePlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}