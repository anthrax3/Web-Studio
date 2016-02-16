using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Prism.Mvvm;
using TreeView.Annotations;

namespace TreeView
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            this.DataContext = vm;
            vm.Paths = @"C:\Users\JORGE\Desktop\hola";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            vm.Paths = @"D:\Captures";
        }
    }
}

