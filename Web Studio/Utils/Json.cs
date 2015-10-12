using System;
using System.IO;
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
                
            }
            return true;
        }

        /// <summary>
        /// Read an object from json file. In case of exception return null
        /// </summary>
        /// <param name="typeOfClass">It's needed to parse the json file</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object FileToObject(object typeOfClass,string path)
        {
            try
            {
                //Try to find the config file
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize(file, typeOfClass.GetType());
                }
            }
            catch (Exception) //FileException or JSonException --> use default values
            {
                return null;
            }
        }
    }


}