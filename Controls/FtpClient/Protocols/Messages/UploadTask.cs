using System.Collections.Generic;
using System.IO;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols.Messages
{
    /// <summary>
    ///     Class to manage upload tasks
    /// </summary>
    public class UploadTask : ProtocolTask
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="itemType"></param>
        public UploadTask(string source, string destination, IProtocolItemType itemType)
        {
            Source = source;
            Destination = destination;
            Type = Strings.Uploads;
            Status = Strings.Pending;
            ItemType = itemType;
        }

        /// <summary>
        ///     Process the message
        /// </summary>
        public override void Process(IProtocol protocol)
        {
            var uploaded = false;
            if (ItemType is FileType)
            {
                uploaded = protocol.UploadFile(Source, Destination);
            }
            if (ItemType is FolderType)
            {
                uploaded = UploadFolder(protocol);
            }
            Status = uploaded ? Strings.Completed : Strings.Error;
        }

        /// <summary>
        ///     Method to upload a folder
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        private bool UploadFolder(IProtocol protocol)
        {
            var uploaded = true;
            var localParentDirectory = PortablePath.GetParentDirectory(Source);
            var remoteParentDirectory = PortablePath.GetParentDirectory(Destination);
            var foldersToProcess = new Queue<string>();
            foldersToProcess.Enqueue(Source);
            while (foldersToProcess.Count > 0)
            {
                var folderToPress = foldersToProcess.Dequeue();
                //Create the folder
                protocol.CreateDirectory(PortablePath.ReplaceAndNormalize(folderToPress, localParentDirectory,
                    remoteParentDirectory));

                //Upload folder files
                var filesToProcess = Directory.GetFiles(folderToPress);
                foreach (var file in filesToProcess)
                {
                    uploaded = uploaded &&
                               protocol.UploadFile(file,
                                   PortablePath.ReplaceAndNormalize(file, localParentDirectory, remoteParentDirectory));
                }

                //Enqueue subfolders
                var subFolders = Directory.GetDirectories(folderToPress);
                foreach (var folder in subFolders)
                {
                    foldersToProcess.Enqueue(folder);
                }
            }
            return uploaded;
        }
    }
}