using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FtpClient.Protocols.ItemTypes;
using FtpClient.Protocols.Messages;

namespace FtpClient
{
    /// <summary>
    /// Code Behind FtpClientUI.xaml
    /// </summary>
    public partial class FtpClientUI : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FtpClientUI()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Selection changed in local explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalSelectionChanged(object sender, RoutedEventArgs e)
        {
            var data = e as SelectionChangedEventArgs;
            var items = ((ViewModel)DataContext).LocalSelectedItems;
            if (data != null)
            {
                items.AddRange(data.AddedItems.Cast<ProtocolItem>());
                foreach (var removedItem in data.RemovedItems)
                {
                    items.Remove((ProtocolItem)removedItem);
                }
            }
        } 

        /// <summary>
        ///     Selection changed in remote explorewr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoteSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = e;
            var items = ((ViewModel)DataContext).RemoteSelectedItems;
            if (data != null)
            {
                items.AddRange(data.AddedItems.Cast<ProtocolItem>());
                foreach (var removedItem in data.RemovedItems)
                {
                    items.Remove((ProtocolItem)removedItem);
                }
            }
        }

        /// <summary>
        ///     Selection changed in tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TasksSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = e;
            var items = ((ViewModel)DataContext).SelectedTasks;
            if (data != null)
            {
                items.AddRange(data.AddedItems.Cast<ProtocolTask>());
                foreach (var removedItem in data.RemovedItems)
                {
                    items.Remove((ProtocolTask)removedItem);
                }
            }
        }  
      
    }
}
