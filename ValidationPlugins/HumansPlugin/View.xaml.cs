namespace HumansPlugin
{
    /// <summary>
    /// Code Behind View.xaml
    /// </summary>
    public partial class View 
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(HumansPlugin vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
