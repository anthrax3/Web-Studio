using System;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Web_Studio.Editor.SyntaxHighlighter
{
    /// <summary>
    ///     Class to manage SyntaxHighlighter easily
    /// </summary>
    public class SyntaxHighlighterTool
    {
        private static IHighlightingDefinition _cssDefinition;
        private static IHighlightingDefinition _jsDefinition;
        private static IHighlightingDefinition _htmlDefinition;

        /// <summary>
        ///     Default constructor for loading custom SyntaxHighlighter
        /// </summary>
        public SyntaxHighlighterTool()
        {
            if (_cssDefinition == null)
            {
                using (var reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory+"Editor/SyntaxHighlighter/CSS.xshd"))
                {
                    _cssDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            if (_jsDefinition == null)
            {
                using (var reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory+"Editor/SyntaxHighlighter/JS.xshd"))
                {
                    _jsDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            if (_htmlDefinition == null)
            {
                using (var reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory+"Editor/SyntaxHighlighter/HTML.xshd"))
                {
                    _htmlDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        /// <summary>
        ///     Get the SyntaxHighlighting for the file extension
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public IHighlightingDefinition SyntaxHighlightingMode(string pathToFile)
        {
            var extension = Path.GetExtension(pathToFile);
            switch (extension)
            {
                case ".html":
                case ".HTML":
                    return _htmlDefinition;
                case ".css":
                case ".CSS":
                    return _cssDefinition;
                case ".ws":
                case ".js":
                case ".JS":
                    return _jsDefinition;
                default:
                    return null; //Only plain text without SyntaxHighlighting
            }
        }
    }
}