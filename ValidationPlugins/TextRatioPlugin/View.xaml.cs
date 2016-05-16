namespace TextRatioPlugin
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
        public View(TextRatioPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}