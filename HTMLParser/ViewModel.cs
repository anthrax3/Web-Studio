using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using FastObservableCollection;
using Prism.Commands;
using Prism.Mvvm;
using ValidationInterface;

namespace HTMLParser
{
    public class ViewModel : BindableBase
    {
       public DelegateCommand PauseCommand { get; private set; }

        public ViewModel()
        {
            PauseCommand = new DelegateCommand(Hello);
        }

        private void Hello()
        {
            Console.WriteLine("AAAAAAAAAAAAA");
        }
    }
}