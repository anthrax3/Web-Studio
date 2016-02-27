using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace HTMLParser
{
    /// <summary>
    ///     Class for loading plugins in MEF arch
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericMefPluginLoader<T>
    {
        /// <summary>
        ///     The composition container for MEF
        /// </summary>
        private readonly CompositionContainer _container;

        /// <summary>
        ///     Loader method
        /// </summary>
        /// <param name="path"></param>
        public GenericMefPluginLoader(string path)
        {
            var directoryCatalog = new DirectoryCatalog(path);

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog(directoryCatalog);

            // Create the CompositionContainer with all parts in the catalog (links Exports and Imports)
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            _container.ComposeParts(this);
        }

        /// <summary>
        ///     List of Plugins that implements T type
        /// </summary>
        [ImportMany]
        public IEnumerable<T> Plugins { get; set; }
    }
}