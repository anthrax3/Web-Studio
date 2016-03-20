using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

// Thanks to Jehof and StackOverflow http://stackoverflow.com/a/13303245/5684370

namespace FastObservableCollection
{
    /// <summary>
    ///     An observable collection with better performance in bulk addition
    /// </summary>
    public class FastObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     it only fires Collection changed event one time, instead of one time for each item
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Items.Add(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}