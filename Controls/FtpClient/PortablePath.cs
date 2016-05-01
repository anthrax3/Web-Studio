using System;

namespace FtpClient
{
    /// <summary>
    ///     Custom class to manage Paths in Windows and Unix filesystems
    /// </summary>
    public class PortablePath
    {
        /// <summary>
        ///     Get the parent directory of the current path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetParentDirectory(string path)
        {
            int lastSeparator;
            if (IsWindowsFileSystem(path))
            {
                lastSeparator = path.LastIndexOf('\\');

                return path.Remove(lastSeparator);
            }
            //Unix
            lastSeparator = path.LastIndexOf('/');
            if (lastSeparator != 0 && lastSeparator == path.Length - 1)
                // it's using /home/myUser the parent folder is /home
            {
                lastSeparator = path.Remove(lastSeparator).LastIndexOf("/", StringComparison.Ordinal);
            }
            if (lastSeparator == 0) // it's using /home the parent folder is /
            {
                return "/";
            }
            return path.Remove(lastSeparator);
        }

        /// <summary>
        ///     Replace oldPath for newPath in path, and normalize the path with the newPath folder separator
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static string ReplaceAndNormalize(string path, string oldPath, string newPath)
        {
            var result = path.Replace(oldPath, newPath);
            if (IsWindowsFileSystem(newPath))
            {
                return result.Replace("/", @"\");
            }
            return result.Replace(@"\", "/");
        }

        /// <summary>
        ///     Check if the path is a windows path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsWindowsFileSystem(string path)
        {
            return path.Contains(@":\");
        }

        /// <summary>
        ///     Return the path separator for the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathSeparator(string path)
        {
            return IsWindowsFileSystem(path) ? @"\" : "/";
        }
    }
}