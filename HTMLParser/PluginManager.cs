using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using ValidationInterface;

namespace HTMLParser
{
    public class PluginManager
    {
        public void LoadAndSortValidationPlugins(string path)
        {
            GenericMefPluginLoader<Lazy<IValidation, IValidationMetadata>> loader = new GenericMefPluginLoader<Lazy<IValidation, IValidationMetadata>>("path");
        }

        public void Test()
        {
            List<IValidationMetadata> lista = new List<IValidationMetadata>();
            lista.Add(new Meta("Include","Error404"));
            lista.Add(new Meta("Error404","Start"));
            lista.Add(new Meta("Robot.txt","Start"));
            lista.Add(new Meta("Humans","Start"));
            lista.Add(new Meta("Sitemap","Start"));
            lista.Add(new Meta("Tw","Include"));
            lista.Add(new Meta("Fb","Include"));
            lista.Add(new Meta("Headers","Fb"));
            lista.Add(new Meta("CssInline","Fb"));
            lista.Add(new Meta("W3C","Headers"));
            lista.Add(new Meta("LinkRoto","W3C"));

            List<IValidationMetadata> ordenada = Ordenar("Start",lista);
            Console.WriteLine(ordenada.ToString());
        }

        private List<IValidationMetadata> Ordenar(string start,List<IValidationMetadata> list )
        {
            Queue<IValidationMetadata> cola = new Queue<IValidationMetadata>();
            cola.Enqueue(new Meta("Start",""));
            List<IValidationMetadata> ordenada = new List<IValidationMetadata>();
            while (cola.Count > 0)
            {
                var elemento = cola.Dequeue();
                ordenada.Add(elemento);
                var asociados = list.Where(t => t.After.Equals(elemento.Name));
                foreach (var asociado in asociados)
                {
                   cola.Enqueue(asociado); 
                }
            }

            return ordenada; 
        }
    }

    public class Meta : IValidationMetadata
    {
        public Meta(string name, string after)
        {
            Name = name;
            After = after;
        }
        /// <summary>
        ///     Plugin name one word only
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Name of the plugin after its execution we have to execute this plugin
        /// </summary>
        public string After { get; set; }
    }
}