namespace W3cPlugin
{
    /// <summary>
    ///     Code Behind View.xaml
    /// </summary>
    public partial class View
    {
        /// <summary>
        /// Default constructor, inject ViewModel
        /// </summary>
        /// <param name="vm"></param>
        public View(W3cPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}