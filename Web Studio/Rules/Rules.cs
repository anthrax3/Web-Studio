using System;
using System.Collections.Generic;

namespace Web_Studio.Reglas
{
    /// <summary>
    ///     Class to manage the rules
    /// </summary>
    internal class Rules
    {
        public List<RulesLanguage> Text { get; set; }
        public string RunToCheck { get; set; }
        public string RunToFix { get; set; }
        public string IsValid { get; set; }
        public string IsWarning { get; set; }

        public void Funcion(string a)
        {
            Console.Write(a);
        }
    }
}