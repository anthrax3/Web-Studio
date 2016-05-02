using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Web_Studio.PluginManager
{
    /// <summary>
    ///     Class for loading plugins in MEF arch
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericMefPluginLoader<T>
    {
        /// <summary>
        ///     Loader method
        /// </summary>
        /// <param name="path"></param>
        public GenericMefPluginLoader(string path)
        {
            var appDirectory  = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (appDirectory != null) path = Path.Combine(appDirectory, path);

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            RecursivedMefPluginLoader(catalog,path);


            // Create the CompositionContainer with all parts in the catalog (links Exports and Imports)
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(this);
        }

        /// <summary>
        ///     List of Plugins that implements T type
        /// </summary>
        [ImportMany]
        public IEnumerable<T> Plugins { get; set; }

        /// <summary>
        ///  Recursived plugin loader
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="path"></param>
        private void RecursivedMefPluginLoader(AggregateCatalog catalog, string path)
        {
            Queue<string> directories = new Queue<string>();
            directories.Enqueue(path);
            while (directories.Count > 0)
            {
                var directory = directories.Dequeue();
                //Load plugins in this folder
                var directoryCatalog = new DirectoryCatalog(directory);
                catalog.Catalogs.Add(directoryCatalog);

                //Add subDirectories to the queue
                var subDirectories = Directory.GetDirectories(directory);
                foreach (string subDirectory in subDirectories)
                {
                    directories.Enqueue(subDirectory);
                }
            }
        }
    }
}