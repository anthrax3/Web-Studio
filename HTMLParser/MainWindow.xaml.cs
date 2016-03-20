using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;
using HtmlAgilityPack;
using ValidationInterface;

namespace HTMLParser
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [ImportMany(typeof(IValidation))]
        IEnumerable<Lazy<IValidation, IValidationMetadata>> _stateRules;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
             HtmlDocument doc = new HtmlDocument();
            doc.Load("file.html");
            var result = doc.DocumentNode.SelectNodes("//include");
            foreach (HtmlNode htmlNode in result)    //Search for include tags
            {
                var resul = htmlNode.GetAttributeValue("src", null);
                if (resul == null)
                {
                    //Error MSG
                }
                else
                {
                    var documentToInclude  = File.ReadAllText(resul);
                    var newNode = HtmlNode.CreateNode(documentToInclude);
                    htmlNode.ParentNode.ReplaceChild(newNode, htmlNode);
                }
            }
            doc.Save("FileProcess.html");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Lazy<IValidation, IValidationMetadata> stateRule in _stateRules)
            {
                Console.WriteLine(stateRule.Metadata.After+"|"+ stateRule.Metadata.Name);
            }
        }
    }
}

