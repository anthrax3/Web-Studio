using System;

namespace FtpClient.Protocols.ItemTypes
{
    /// <summary>
    ///     Abstract file system item
    /// </summary>
    public class ProtocolItem
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullPath"></param>
        /// <param name="size"></param>
        /// <param name="lastWrite"></param>
        /// <param name="type"></param>
        public ProtocolItem(string name, string fullPath, long size, DateTime lastWrite, IProtocolItemType type)
        {
            Name = name;
            FullPath = fullPath;
            Size = size;
            LastWrite = lastWrite;
            Type = type;
        }

        /// <summary>
        ///     Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of the item
        /// </summary>
        public IProtocolItemType Type { get; set; }

        /// <summary>
        ///     Size of the item in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        ///     Last time when the item was written
        /// </summary>
        public DateTime LastWrite { get; set; }

        /// <summary>
        ///     Full path of the item
        /// </summary>
        public string FullPath { get; set; }
    }
}