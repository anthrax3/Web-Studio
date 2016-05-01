using System;
using System.Collections.Generic;
using System.IO;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols.Messages
{
    /// <summary>
    ///     Task for Download an item
    /// </summary>
    public class DownloadTask : ProtocolTask
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="itemType"></param>
        public DownloadTask(string source, string destination, IProtocolItemType itemType)
        {
            Type = Strings.Downloads;
            Source = source;
            Destination = destination;
            Status = Strings.Pending;
            ItemType = itemType;
        }

        /// <summary>
        ///     Process the message
        /// </summary>
        public override void Process(IProtocol protocol)
        {
            var downloaded = false;
            if (ItemType is FileType)
            {
                downloaded = protocol.DownloadFile(Source, Destination);
            }
            if (ItemType is FolderType)
            {
                downloaded = DownloadFolder(protocol);
            }

            Status = downloaded ? Strings.Completed : Strings.Error;
        }

        /// <summary>
        ///     Metho to download a folder
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        private bool DownloadFolder(IProtocol protocol)
        {
            var downloaded = true;
            var sourceParentDirectory = PortablePath.GetParentDirectory(Source);
            var destinationParentDirectory = PortablePath.GetParentDirectory(Destination);
            var foldersToProcess = new Queue<string>();
            foldersToProcess.Enqueue(Source);
            while (foldersToProcess.Count > 0)
            {
                var folderToPress = foldersToProcess.Dequeue();
                //Create the folder
                try
                {
                    Directory.CreateDirectory(PortablePath.ReplaceAndNormalize(folderToPress, sourceParentDirectory,
                        destinationParentDirectory));
                }
                catch (Exception) //Error when creating directory
                {
                    return false;
                }


                var itemToProcess = protocol.ListDirectory(folderToPress);
                foreach (var item in itemToProcess)
                {
                    if (item.Type is FileType) //Download folder files
                    {
                        downloaded = downloaded &&
                                     protocol.DownloadFile(item.FullPath,
                                         PortablePath.ReplaceAndNormalize(item.FullPath, sourceParentDirectory,
                                             destinationParentDirectory));
                    }
                    if (item.Type is FolderType) //Enqueue subfolders
                    {
                        foldersToProcess.Enqueue(item.FullPath);
                    }
                }
            }
            return downloaded;
        }
    }
}