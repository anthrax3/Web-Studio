﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LinksPlugin.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LinksPlugin.Properties.Strings", typeof(Strings).Assembly);
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
        ///   Busca una cadena traducida similar a That url seems broken.
        /// </summary>
        internal static string BrokenUrl {
            get {
                return ResourceManager.GetString("BrokenUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a This file has no references, so you can remove it.
        /// </summary>
        internal static string CanRemoveFile {
            get {
                return ResourceManager.GetString("CanRemoveFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a This plugin checks if all links work and if all project files are linked.
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Introduce the full path to root folder, example http://google.es.
        /// </summary>
        internal static string DomainName {
            get {
                return ResourceManager.GetString("DomainName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You need to fill the domain field.
        /// </summary>
        internal static string DomainNotFound {
            get {
                return ResourceManager.GetString("DomainNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The url should be shorter.
        /// </summary>
        internal static string Length {
            get {
                return ResourceManager.GetString("Length", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a The local file doesn&apos;t exists.
        /// </summary>
        internal static string LocalFileNotFound {
            get {
                return ResourceManager.GetString("LocalFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Links.
        /// </summary>
        internal static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a You should remove &quot;?&quot; from your urls. That improves the SEO.
        /// </summary>
        internal static string QuestionMark {
            get {
                return ResourceManager.GetString("QuestionMark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a This url should only use english letters, numbers and hyphen.
        /// </summary>
        internal static string SeoUrl {
            get {
                return ResourceManager.GetString("SeoUrl", resourceCulture);
            }
        }
    }
}
