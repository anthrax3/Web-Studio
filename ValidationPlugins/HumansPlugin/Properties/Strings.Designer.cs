﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HumansPlugin.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HumansPlugin.Properties.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Fix automatically.
        /// </summary>
        internal static string AutoFix {
            get {
                return ResourceManager.GetString("AutoFix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You need to fill all data in plugin config or your may get a bad humans.txt.
        /// </summary>
        internal static string DataNeeded {
            get {
                return ResourceManager.GetString("DataNeeded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a This plugin checks for humans.txt file. If the file doesn&apos;t exist it can create it.
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Humans generated correctly.
        /// </summary>
        internal static string Generated {
            get {
                return ResourceManager.GetString("Generated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Humans.txt.
        /// </summary>
        internal static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a File not found.
        /// </summary>
        internal static string NotFound {
            get {
                return ResourceManager.GetString("NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Introduce team members info..
        /// </summary>
        internal static string TeamText {
            get {
                return ResourceManager.GetString("TeamText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Introduce the technology used.
        /// </summary>
        internal static string TechnologyText {
            get {
                return ResourceManager.GetString("TechnologyText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Introduce the thanks.
        /// </summary>
        internal static string ThanksText {
            get {
                return ResourceManager.GetString("ThanksText", resourceCulture);
            }
        }
    }
}
