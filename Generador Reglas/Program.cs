using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Newtonsoft.Json;
using Web_Studio.Reglas;

namespace Generador_Reglas
{
    class Program
    {
        static void Main(string[] args)
        {
            var regla = new Rules
            {
                RunToCheck= "var resultado = file.exit=robots.txt",
                RunToFix = "file.create(robots.txt)",
                IsValid = "return resultado",
                IsWarning = "false" ,
                Text = new List<RulesLanguage>()
            };

            regla.Text.Add( new RulesLanguage
            {
                Title = "Fichero Robots.txt" ,
                Description = "Comprobamos si hay un fichero Robots.txt",
                Language = "es-ES",
                SuggestedSolution = "Crear fichero robots.txt para que los buscadores te puedan indexar",
                Type = "Desarrollo"
            });

            regla.Text.Add(new RulesLanguage
            {
                Title = "File Robots.txt",
                Description = "Check if robots.txt exists",
                Language = "en-US",
                SuggestedSolution = "You have to create a robots.txt file for indexing",
                Type = "Develop"
                
            });

            string output = JsonConvert.SerializeObject(regla);
            File.WriteAllText("json",output);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

          
   /*         if (resultado.Errors.Count != 0)
            {
                resultado.CompiledAssembly.CreateInstance()
            }
            */
        }
    }
}
