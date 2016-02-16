using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace SocialTest
{
    public class GenericMEFPluginLoader<T>
    {
        private readonly CompositionContainer _Container;

        public GenericMEFPluginLoader(string path)
        {
            var directoryCatalog = new DirectoryCatalog(path);

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog(directoryCatalog);

            // Create the CompositionContainer with all parts in the catalog (links Exports and Imports)
            _Container = new CompositionContainer(catalog);

            //Fill the imports of this object
            _Container.ComposeParts(this);
        }

        [ImportMany]
        public IEnumerable<T> Plugins { get; set; }
    }
}