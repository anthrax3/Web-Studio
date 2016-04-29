using System.ComponentModel;
using System.Runtime.CompilerServices;
using FtpClient.Annotations;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols.Messages
{
    /// <summary>
    ///     Abstract task
    /// </summary>
    public abstract class ProtocolTask : INotifyPropertyChanged
    {
        private string _status;

        /// <summary>
        ///     Path to destionation
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        ///     Path to source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///     Status of message
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Type of message
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     The Item type (folder or file)
        /// </summary>
        public IProtocolItemType ItemType { get; set; }

        /// <summary>
        ///     Event for MVVM binding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Process the message
        /// </summary>
        public abstract void Process(IProtocol protocol);

        /// <summary>
        ///     MVVM binding
        /// </summary>
        /// <param name="propertyName"></param>   
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}