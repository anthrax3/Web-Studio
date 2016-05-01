using System;
using System.Collections.Generic;
using FtpClient.Protocols.ItemTypes;

namespace FtpClient.Protocols.FTP
{
    /// <summary>
    ///     Class for parse the result of ftp list directory (ls -l) to Protocol Item
    /// </summary>
    public static class FtpParser
    {
        /// <summary>
        ///     Method for parse the ls -l output
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        public static List<ProtocolItem> Parse(string data, string currentPath)
        {
            var lines = data.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            var items = new List<ProtocolItem>();
            foreach (var line in lines)
            {
                items.Add(ItemParser(line, currentPath));
            }
            return items;
        }

        /// <summary>
        ///     Method for parse a line of the ls -l output
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        private static ProtocolItem ItemParser(string item, string currentPath)
        {
            var parts = item.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);


            var date = DateParser(parts[5], parts[6], parts[7]);
            if (parts[0][0] == 'd') //Folder
            {
                return new ProtocolItem(parts[8], currentPath + PortablePath.PathSeparator(currentPath) + parts[8], 0,
                    date, FolderType.Instance);
            }
            if (parts[0][0] == '-') //File
            {
                return new ProtocolItem(parts[8], currentPath + PortablePath.PathSeparator(currentPath) + parts[8],
                    Convert.ToInt64(parts[4]), date, FileType.Instance);
            }
            return new ProtocolItem("aaa", "aaa", 0, DateTime.Today, FileType.Instance);
        }

        /// <summary>
        ///     Method to get a date from ls -l output (Apr 28 15:55)
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hourAndMinutes"></param>
        /// <returns></returns>
        private static DateTime DateParser(string month, string day, string hourAndMinutes)
        {
            var monthNumber = 1;
            switch (month)
            {
                case "Jan":
                    monthNumber = 1;
                    break;
                case "Feb":
                    monthNumber = 2;
                    break;
                case "Mar":
                    monthNumber = 3;
                    break;
                case "Apr":
                    monthNumber = 4;
                    break;
                case "May":
                    monthNumber = 5;
                    break;
                case "Jun":
                    monthNumber = 6;
                    break;
                case "Jul":
                    monthNumber = 7;
                    break;
                case "Aug":
                    monthNumber = 8;
                    break;
                case "Sep":
                    monthNumber = 9;
                    break;
                case "Oct":
                    monthNumber = 10;
                    break;
                case "Nov":
                    monthNumber = 11;
                    break;
                case "Dec":
                    monthNumber = 12;
                    break;
            }
            var dayNumber = Convert.ToInt32(day);
            var parts = hourAndMinutes.Split(':');
            var hoursNumber = Convert.ToInt32(parts[0]);
            var minutesNumber = Convert.ToInt32(parts[1]);
            return new DateTime(DateTime.Today.Year, monthNumber, dayNumber, hoursNumber, minutesNumber, 0);
        }
    }
}