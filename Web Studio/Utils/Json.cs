using System;
using System.IO;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Web_Studio.Utils
{
    /// <summary>
    /// Class to manage importation and exportation in JSON format
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Write an object in JSON format in a path
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ObjectToFile(object myObject, string path)
        {
            try
            {
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented };
                    serializer.Serialize(file, myObject);
                }
            }
            catch (Exception)
            {
                return false;                
                throw;
            }
            return true;
        }
    }


}