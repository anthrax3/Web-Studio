using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using ValidationInterface;

namespace HTMLParser
{
    public class ViewModel : BindableBase
    {
        public ObservableCollection<AnalysisResult> Results { get; set; }  = new ObservableCollection<AnalysisResult>();
        public DelegateCommand CheckCommand { get; private set; }
        public ViewModel()
        {
            CheckCommand = new DelegateCommand(Check);
        }

   

        private void Check()
        {
            Results.Clear();

            var loader = new GenericMefPluginLoader<IValidation>("Plugins");
            var path = @"D:\abc\Check";
            foreach (IValidation plugin in loader.Plugins)
            {
                Results.AddRange(plugin.Check(path));
            }

        }
    }
}