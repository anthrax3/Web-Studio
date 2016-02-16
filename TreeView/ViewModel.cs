using Prism.Mvvm;

namespace TreeView
{
    public class ViewModel : BindableBase
    {
        private string _pathS;
        public string Paths
        {
            get { return _pathS; }
            set { SetProperty(ref _pathS, value); }
        }

    }
}