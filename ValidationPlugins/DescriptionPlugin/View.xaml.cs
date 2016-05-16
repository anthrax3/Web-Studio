namespace DescriptionPlugin
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
        public View(DescriptionPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}