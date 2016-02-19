using Prism.Mvvm;

namespace AvalonDockTest
{
    public class EditorViewModel : DockWindowViewModel
    {
        private string _textoToShow;
        public string TextToShow
        {
            get { return _textoToShow; }
            set { SetProperty(ref _textoToShow, value); }
        }

    }
}