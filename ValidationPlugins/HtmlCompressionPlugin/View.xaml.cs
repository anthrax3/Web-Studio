﻿using System.Windows.Controls;

namespace HtmlCompressionPlugin
{
    /// <summary>
    ///     Lógica de interacción para View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="vm"></param>
        public View(HtmlCompression vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}