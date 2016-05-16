namespace HeadingPlugin
{
    /// <summary>
    ///     Code Behind View.xaml
    /// </summary>
    public partial class View 
    {
        /// <summary>
        ///     Default constructor, inject ViewModel
        /// </summary>
        /// <param name="vm"></param>
        public View(HeadingPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}