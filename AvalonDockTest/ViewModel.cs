using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace AvalonDockTest
{
    public class ViewModel : BindableBase
    {
  
        public ViewModel()
        {
            Documents = new ObservableCollection<DockWindowViewModel>();
            Documents.Add(new EditorViewModel
            {
                Title = "Sample",
                ToolTip = "C/fichero",
                TextToShow = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            });
            Documents.Add(new EditorViewModel
            {
                Title = "asdfasdf",
                ToolTip = "C/ficheasdfaro",
                TextToShow = "asdasdasdasdasdasdasdasdasd"
            });
        }

        public ObservableCollection<DockWindowViewModel> Documents { get; private set; }

        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                SetProperty(ref _itemName, value);
            }
        }

        private void ProcessChange()
        {
            if (!Documents.Any(doc => doc.ToolTip == ItemPath) && !ItemIsFolder)
            {
                Documents.Add(new EditorViewModel() {Title = ItemName, ToolTip = ItemPath});
            }

        }

        private string _itemPath;
        public string ItemPath
        {
            get { return _itemPath; }
            set { SetProperty(ref _itemPath, value); }
        }

        private bool _itemisFolder;
        public bool ItemIsFolder
        {
            get { return _itemisFolder; }
            set
            {
                SetProperty(ref _itemisFolder, value);
                ProcessChange();

            }
        }
    }

    public class DockWindowViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _toolTip;
        public string ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
        }

    }
}