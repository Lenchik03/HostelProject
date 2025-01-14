﻿using HostelProject.mvvm.view;
using HostelProject.mvvm.viewmodel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HostelProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Manager(object sender, MouseButtonEventArgs e)
        {
            MainVM.Instance.CurrentPage = new MainPage(MainVM.Instance);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            
        }
    }
}